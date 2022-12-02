using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Repository.DBContext;

namespace ControleFinanceiro.Repository.Repositories;

public class TransactionRepository : AbstractRepository<TransactionViewModel>
{
    public TransactionRepository(FinancialControlContext context)
        : base(context)
    {
    }
}