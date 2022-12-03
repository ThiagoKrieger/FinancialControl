using Domain.Abstractions;

namespace Repository.Abstractions;

public interface IRepository<TEntity> : IDisposable
    where TEntity : IEntity
{
    public Task<TEntity?> GetByKeyAsync(int key, CancellationToken token);

    Task<IEnumerable<TEntity>> GetAsync(CancellationToken token);

    Task UpdateAsync(TEntity entity, CancellationToken token);

    Task<bool> AddAsync(TEntity entity, CancellationToken token);
    Task<bool> RemoveAsync(int id, CancellationToken token);
}