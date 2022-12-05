using ControleFinanceiro.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace WebApplication1.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _repository;
    private readonly IUserBusinessLogic _businessLogic;

    public UserController(IUserRepository userRepository, IUserBusinessLogic businessLogic)
    {
        _repository = userRepository;
        _businessLogic = businessLogic;
    }

    // GET: User
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userList = (await _repository.GetAllUsersWithTransactions(cancellationToken)).ToList();

        var financialInfo = new Dictionary<int, Tuple<float, float>>();
        
        foreach (var user in userList.Where(user => user is not null))
        {
            var incomesAndOutcomes = await _businessLogic.GetIncomeAndOutcome(user!.Id, cancellationToken);
            financialInfo.Add(user.Id, incomesAndOutcomes);
        }

        ViewBag.FinancialInfo = financialInfo;
        return Ok(userList);
    }

    // GET: User/Details/5
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserWithTransactions(id, cancellationToken);

        return Ok(user);
    }

    // POST: User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Age,Name,Balance")] User user,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(user);

        if (!await _repository.AddAsync(user, cancellationToken))
            return Problem($"Wasn't able to save {user.Name}");

        return RedirectToAction(nameof(Index));
    }

    // PATCH: User/Edit/5
    [HttpPatch]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Age,Name,Balance")] User user,
        CancellationToken cancellationToken)
    {
        if (id != user.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(user);
        try
        {
            await _repository.UpdateAsync(user, cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserViewModelExists(user.Id, cancellationToken))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // DELETE: User/Delete/5
    [HttpDelete, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        return await _repository.RemoveAsync(id, cancellationToken)
            ? RedirectToAction(nameof(Index))
            : Problem("There is no such entity in database to be deleted");
    }

    private async Task<bool> UserViewModelExists(int id, CancellationToken token)
    {
        return await _repository.GetByKeyAsync(id, token) is not null;
    }
}