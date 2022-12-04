using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace WebApplication1.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionRepository _repository;
        private readonly IUserDataProvider _dataProvider;

        public TransactionsController(ITransactionRepository repository, IUserDataProvider dataProvider)
        {
            _repository = repository;
            _dataProvider = dataProvider;
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
        public async Task<IActionResult> Create([Bind("Description,Value,Type,UserId")] Transaction transaction, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(transaction);
            
            if(!await _repository.AddAsync(transaction, token))
                return Problem($"Wasn't able to save the transaction {transaction.Description}");
            
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Value,Type,UserId")] Transaction transaction, CancellationToken token)
        {
            if (id != transaction.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(transaction);
            try
            {
                await _repository.UpdateAsync(transaction, token);
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
            if(!await _repository.RemoveAsync(id, token))
                return Problem("Wasn't able to delete the transaction");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TransactionViewModelsExists(int id, CancellationToken token)
        {
          return await _repository.GetByKeyAsync(id, token) is not null;
        }
    }
}
