using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Repository.DBContext;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.Repositories;

public class UserRepository : AbstractRepository<UserViewModel>, IUserRepository
{
    public UserRepository(FinancialControlContext context)
        : base(context)
    {
    }
}