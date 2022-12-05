using ControleFinanceiro.Domain.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;
using WebApplication1.Contracts.Request;
using WebApplication1.Contracts.Transformations;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionsController : Controller
{
    private readonly ITransactionRepository _repository;
    private readonly IValidator<Transaction> _validator;
    private readonly ITransactionToResponseTransformation _toResponseTransformation;
    private readonly IRequestToTransactionTransformation _fromRequestTransformation;
    private readonly IUserBusinessLogic _businessLogic;

    public TransactionsController(ITransactionRepository repository,
        IValidator<Transaction> validator,
        ITransactionToResponseTransformation toResponseTransformation,
        IRequestToTransactionTransformation fromRequestTransformation,
        IUserBusinessLogic businessLogic)
    {
        _repository = repository;
        _validator = validator;
        _toResponseTransformation = toResponseTransformation;
        _fromRequestTransformation = fromRequestTransformation;
        _businessLogic = businessLogic;
    }

    /// <summary>
    /// Return all the transactions
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken token)
    {
        var transactions = await _repository.GetAsync(token);
        var responses = await _toResponseTransformation.TransformToMany(transactions, token);
        return Ok(responses);
    }

    /// <summary>
    /// This return a specific transaction
    /// </summary>
    [HttpGet("details")]
    public async Task<IActionResult> Details(int id, CancellationToken token)
    {
        var transaction = await _repository.GetByKeyAsync(id, token);
        if (transaction is null)
            return NotFound();
        
        var response = await _toResponseTransformation.TransformTo(transaction, token);

        return Ok(response);
    }

    /// <summary>
    /// This create a new Transaction
    /// </summary>
    /// <remarks>
    ///   Possible transaction values:
    ///         Income,
    ///         Outcome
    /// </remarks>
    [HttpPost("create")]
    public async Task<IActionResult> Create(TransactionRequest transactionRequest,
        CancellationToken token)
    {
        var transaction = await _fromRequestTransformation.TransformTo(transactionRequest, token);
        var result = await _validator.ValidateAsync(transaction, token);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        if (!await _repository.AddAsync(transaction, token))
            return Problem($"Wasn't able to save the transaction {transaction.Description}");

        await _businessLogic.SetBalances(transaction.UserId, token);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// This edits a existing Transaction
    /// </summary>
    /// <remarks>
    ///   Possible transaction values:
    ///         Income,
    ///         Outcome
    /// </remarks>
    [HttpPatch]
    public async Task<IActionResult> Edit(int id,
        TransactionRequest transactionRequest,
        CancellationToken token)
    {
        var transaction = await _repository.GetByKeyAsync(id, token);
        if (transaction is null)
            return NotFound(transactionRequest);
        await _fromRequestTransformation.TransformTo(transactionRequest, transaction, token);
        var result = await _validator.ValidateAsync(transaction, token);
        
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        try
        {
            await _repository.UpdateAsync(transaction, token).ConfigureAwait(false);
            await _businessLogic.SetBalances(transaction.UserId, token).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TransactionViewModelsExists(transaction.Id, token))
                return NotFound();

            throw;
        }

        return await Index(token);
    }

    /// <summary>
    /// This deletes a existing Transaction
    /// </summary>
    /// <remarks>
    ///   Possible transaction values:
    ///         Income,
    ///         Outcome
    /// </remarks>
    [HttpDelete, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken token)
    {
        var toDelete = await _repository.GetByKeyAsync(id, token);
        var userToUpdate = toDelete?.UserId ?? 0;

        if (!await _repository.RemoveAsync(id, token))
            return Problem("Wasn't able to delete the transaction");
        if (userToUpdate != 0)
            await _businessLogic.SetBalances(userToUpdate, token).ConfigureAwait(false);
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> TransactionViewModelsExists(int id, CancellationToken token)
    {
        return await _repository.GetByKeyAsync(id, token) is not null;
    }
}