using ControleFinanceiro.Domain.Models;
using Repository.Abstractions;
using WebApplication1.Contracts.Request;
using WebApplication1.Contracts.Response;

namespace WebApplication1.Contracts.Transformations;

public class RequestToUserTransformation : AbstractTransformation<UserRequest, User>, IRequestToUserTransformation
{
}

public class UserToResponseToResponseTransformation : AbstractTransformation<User, UserResponse>, IUserToResponseTransformation
{
    private readonly IUserBusinessLogic _userBusinessLogic;

    public UserToResponseToResponseTransformation(IUserBusinessLogic userBusinessLogic)
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