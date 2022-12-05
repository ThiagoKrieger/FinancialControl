using ControleFinanceiro.Repository.DBContext;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.Repositories;

public abstract class AbstractRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly FinancialControlContext Context;

    protected AbstractRepository(FinancialControlContext context)
    {
        Context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAsync(CancellationToken token)
    {
        return await Context.Set<TEntity>().ToListAsync(token);
    }

    public async Task<TEntity?> GetByKeyAsync(int key, CancellationToken token)
    {
        return await Context.Set<TEntity>()
            .FindAsync(new object?[]
                {
                    key, token
                },
                cancellationToken: token);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken token)
    {
        if (await GetByKeyAsync(entity.Id, token) is null)
            return;
        Context.Update(entity);
        await Context.SaveChangesAsync(token);
    }

    public async Task<bool> AddAsync(TEntity entity, CancellationToken token)
    {
        Context.Add(entity);
        SaveChanges();

        return await GetByKeyAsync(entity.Id, token) is not null;
    }

    public async Task<bool> RemoveAsync(int id, CancellationToken token)
    {
        var entity = await GetByKeyAsync(id, token);
        if (entity is null)
            return false;

        Context.Remove(entity);
        SaveChanges();

        return true;
    }

    private void SaveChanges()
    {
        Context.SaveChanges();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Context.Dispose();
    }
}