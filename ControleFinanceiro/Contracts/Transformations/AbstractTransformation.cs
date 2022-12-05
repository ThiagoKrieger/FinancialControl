using Utils;

namespace WebApplication1.Contracts.Transformations;

public abstract class AbstractTransformation<TEntity, TResponse> : ITransformation<TEntity, TResponse>
    where TResponse : class, new()
{
    public virtual Task<TResponse> TransformTo(TEntity? entity, CancellationToken token)
    {
        return Task.FromResult(entity?.ProjectToNew<TResponse>() ?? new TResponse());
    }

    public Task<TResponse> TransformTo(TEntity entity, TResponse result, CancellationToken token)
    {
        entity.ProjectTo(result);
        return Task.FromResult(result);
    }

    public virtual Task<List<TResponse>> TransformToMany(IEnumerable<TEntity?> entities, CancellationToken token)
    {
        var list = entities.Select(entity => TransformTo(entity, token).Result).ToList();

        return Task.FromResult(list);
    }
}