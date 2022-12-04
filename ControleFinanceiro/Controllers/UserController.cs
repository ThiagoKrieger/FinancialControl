﻿using System.Collections;
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
        var userList = await _repository.GetAllUsersWithTransactions(cancellationToken);

        var financialInfo = new Dictionary<int, Tuple<float, float>>();
        
        foreach (var user in userList)
        {
            if (user is null)
                continue;
            var incomesAndOutcomes = await _businessLogic.GetIncomeAndOutcome(user.Id, cancellationToken);
            financialInfo.Add(user.Id, incomesAndOutcomes);
        }

        var teste = financialInfo[userList.First().Id].Item1;
        ViewBag.FinancialInfo = financialInfo;
        return View(userList);
    }

    // GET: User/Details/5
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserWithTransactions(id, cancellationToken);
        if (user is null)
            return NotFound();

        return View(user);
    }

    // GET: User/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Age,Name,Balance")] User user,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(user);

        if (!await _repository.AddAsync(user, cancellationToken))
            return Problem($"Wasn't able to save {user.Name}");

        return RedirectToAction(nameof(Index));
    }

    // GET: User/Edit/5
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByKeyAsync(id, cancellationToken);
        if (user is null)
            return NotFound();

        return View(user);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Age,Name,Balance")] User user,
        CancellationToken cancellationToken)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
            return View(user);
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

    // GET: User/Delete/5
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var userViewModel = await _repository.GetByKeyAsync(id, cancellationToken);
        if (userViewModel is null)
            return NotFound();

        return View(userViewModel);
    }

    // POST: User/Delete/5
    [HttpPost, ActionName("Delete")]
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