using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Domain.Models.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace WebApplication1.Controllers;

public class TransactionsController : Controller
{
    private readonly ITransactionRepository _repository;
    private readonly IValidator<Transaction> _validator;
    private readonly IUserRepository _userRepository;
    private readonly IUserDataProvider _dataProvider;
    private readonly IUserBusinessLogic _businessLogic;

    public TransactionsController(ITransactionRepository repository,
        IValidator<Transaction> validator,
        IUserRepository userRepository,
        IUserDataProvider dataProvider,
        IUserBusinessLogic businessLogic)
    {
        _repository = repository;
        _validator = validator;
        _userRepository = userRepository;
        _dataProvider = dataProvider;
        _businessLogic = businessLogic;
    }

    // GET: Transactions
    public async Task<IActionResult> Index(CancellationToken token)
    {
        return View(await _repository.GetAsync(token));
    }

    // GET: Transactions/Details/5
    public async Task<IActionResult> Details(int id, CancellationToken token)
    {
        var transactionViewModel = await _repository.GetByKeyAsync(id, token);

        if (transactionViewModel is null)
            return NotFound();

        return View(transactionViewModel);
    }

    // GET: Transactions/Create
    public async Task<IActionResult> Create(CancellationToken token)
    {
        var userList = await _dataProvider.GetItems(token);
        ViewBag.ListOfUser = userList;

        return View();
    }

    // POST: Transactions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Description,Value,Type,UserId")] Transaction transaction,
        CancellationToken token)
    {
        var result = await _validator.ValidateAsync(transaction, token);
            
        if (!result.IsValid)
            return View(transaction);

        var relatedUser = await _userRepository.GetByKeyAsync(transaction.UserId, token);
        if (transaction.Type == TransactionType.Income && relatedUser!.Age < 18)
            return Problem("Only over age users can have Incomes. Set the transaction type to outcome or select an ove age user.");
        if (!await _repository.AddAsync(transaction, token))
            return Problem($"Wasn't able to save the transaction {transaction.Description}");

        await _businessLogic.SetBalances(transaction.UserId, token);
        return RedirectToAction(nameof(Index));
    }

    // GET: Transactions/Edit/5
    public async Task<IActionResult> Edit(int id, CancellationToken token)
    {
        var transactionViewModel = await _repository.GetByKeyAsync(id, token);

        if (transactionViewModel is null)
            return NotFound();

        return View(transactionViewModel);
    }

    // POST: Transactions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Description,Value,Type,UserId")] Transaction transaction,
        CancellationToken token)
    {
        var result = await _validator.ValidateAsync(transaction, token);
        if (id != transaction.Id)
            return NotFound();

        if (!result.IsValid)
            return View(transaction);
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

        return RedirectToAction(nameof(Index));
    }

    // GET: Transactions/Delete/5
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        var transactionViewModel = await _repository.GetByKeyAsync(id, token);

        if (transactionViewModel is null)
            return NotFound();

        return View(transactionViewModel);
    }

    // POST: Transactions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken token)
    {
        var toDelete = await _repository.GetByKeyAsync(id, token);
        var userToUpdate = toDelete?.UserId ?? 0;

        if (!await _repository.RemoveAsync(id, token))
            return Problem("Wasn't able to delete the transaction");
        if(userToUpdate != 0)
            await _businessLogic.SetBalances(userToUpdate, token).ConfigureAwait(false);
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> TransactionViewModelsExists(int id, CancellationToken token)
    {
        return await _repository.GetByKeyAsync(id, token) is not null;
    }
}