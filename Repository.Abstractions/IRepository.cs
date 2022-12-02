using Domain.Abstractions;

namespace Repository.Abstractions;

public interface IRepository<TEntity> where TEntity : IEntity
{
    public Task<TEntity?> GetByKeyAsync(int key, CancellationToken token);

    public void SaveChanges();
}