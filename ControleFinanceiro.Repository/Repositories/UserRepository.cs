using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Repository.DBContext;

namespace ControleFinanceiro.Repository.Repositories;

public class UserRepository : AbstractRepository<UserViewModel>
{
    private readonly FinancialControlContext _context;

    public UserRepository(FinancialControlContext context) : base(context)
    {
        _context = context;
    }
}