using ControleFinanceiro.Repository.DBContext;
using Domain.Abstractions;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.Repositories;

public abstract class AbstractRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly FinancialControlContext _context;

    public AbstractRepository(FinancialControlContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByKeyAsync(int key, CancellationToken token)
    {
        return await _context.Set<TEntity>()
            .FindAsync(new object?[]
                {
                    key, token
                },
                cancellationToken: token);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}