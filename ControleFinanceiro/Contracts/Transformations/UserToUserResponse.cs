using ControleFinanceiro.Domain.Models;
using Repository.Abstractions;
using Utils;
using WebApplication1.Contracts.Request;
using WebApplication1.Contracts.Response;

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

public class UserToResponseTransformation : AbstractTransformation<User, UserResponse>, IUserTransformation
{
    private readonly IUserBusinessLogic _userBusinessLogic;

    public UserToResponseTransformation(IUserBusinessLogic userBusinessLogic)
    {
        _userBusinessLogic = userBusinessLogic;
    }
    public override async Task<UserResponse> TransformTo(User? entity, CancellationToken token)
    {
        if (entity == null)
            return new UserResponse();
        
        var incomeAndOutcome = await _userBusinessLogic.GetIncomeAndOutcome(entity.Id, token);
        var response = await base.TransformTo(entity, token);
        response.Income = incomeAndOutcome.Item1;
        response.Outcome = incomeAndOutcome.Item2;

        return response;
    }

    public override async Task<List<UserResponse>> TransformToMany(IEnumerable<User?> entities, CancellationToken token)
    {
        var list = new List<UserResponse>();
        foreach (var entity in entities)
        {
            list.Add(await TransformTo(entity, token));
        }
        return list;
    }
}

public class RequestToUserTransformation : AbstractTransformation<UserRequest, User>, IRequestToUserTransformation
{
    
}