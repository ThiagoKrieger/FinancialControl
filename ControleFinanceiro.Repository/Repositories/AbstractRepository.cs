using ControleFinanceiro.Repository.DBContext;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.Repositories;

public abstract class AbstractRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly FinancialControlContext _context;

    protected AbstractRepository(FinancialControlContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAsync(CancellationToken token)
    {
        return await _context.Set<TEntity>().ToListAsync(token);
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

    public async Task UpdateAsync(TEntity entity, CancellationToken token)
    {
        if (await GetByKeyAsync(entity.Id, token) is not null)
            return;
        _context.Update(entity);
        await _context.SaveChangesAsync(token);
    }

    public async Task<bool> AddAsync(TEntity entity, CancellationToken token)
    {
        _context.Add(entity);
        SaveChanges();

        return await GetByKeyAsync(entity.Id, token) is not null;
    }

    public async Task<bool> RemoveAsync(int id, CancellationToken token)
    {
        var entity = await GetByKeyAsync(id, token);
        if (entity is null)
            return false;

        _context.Remove(entity);
        SaveChanges();

        return true;
    }

    private void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}