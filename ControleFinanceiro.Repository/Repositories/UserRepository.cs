using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Repository.DBContext;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.Repositories;

public class UserRepository : AbstractRepository<User>, IUserRepository
{
    public UserRepository(FinancialControlContext context)
        : base(context)
    {
    }

    public async Task<User> GetUserWithTransactions(int id, CancellationToken token)
    {
        return await Context.Set<User>()
            .Where(user => user.Id == id)
            .Include(user => user.Transactions)
            .SingleAsync(token)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<User?>> GetAllUsersWithTransactions(CancellationToken token)
    {
        return await Context.Set<User>()
            .Include(user => user.Transactions)
            .ToArrayAsync(token)
            .ConfigureAwait(false);
    }
}