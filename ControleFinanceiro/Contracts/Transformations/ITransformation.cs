using ControleFinanceiro.Domain.Models;
using WebApplication1.Contracts.Request;
using WebApplication1.Contracts.Response;

namespace WebApplication1.Contracts.Transformations;

public interface ITransformation<in TEntity, TResult> where TResult : class, new()
{
    public Task<TResult> TransformTo(TEntity entity, CancellationToken token);
    public Task<TResult> TransformTo(TEntity entity, TResult result, CancellationToken token);
    public Task<List<TResult>> TransformToMany(IEnumerable<TEntity?> entities, CancellationToken token);
}

public interface IUserTransformation : ITransformation<User, UserResponse>
{
}

public interface IRequestToUserTransformation : ITransformation<UserRequest, User>
{
}