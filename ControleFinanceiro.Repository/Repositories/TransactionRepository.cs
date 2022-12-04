using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Repository.DBContext;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.Repositories;

public class TransactionRepository : AbstractRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(FinancialControlContext context)
        : base(context)
    {
    }
}