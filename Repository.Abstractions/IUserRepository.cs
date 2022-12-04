using ControleFinanceiro.Domain.Models;

namespace Repository.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserWithTransactions(int id, CancellationToken token);
}