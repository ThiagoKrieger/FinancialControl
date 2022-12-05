using ControleFinanceiro.Domain.Models;
using Repository.Abstractions;
using WebApplication1.Contracts.Request;
using WebApplication1.Contracts.Response;

namespace WebApplication1.Contracts.Transformations;

public class TransactionToResponseTransformation
    : AbstractTransformation<Transaction, TransactionResponse>, ITransactionToResponseTransformation
{
    private readonly IUserRepository _userRepository;

    public TransactionToResponseTransformation(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public override async Task<TransactionResponse> TransformTo(Transaction? entity, CancellationToken token)
    {
        if (entity is null)
            return new TransactionResponse();
        var response = await base.TransformTo(entity, token);
        var relatedUser = await _userRepository.GetByKeyAsync(entity.UserId, token);
        if (relatedUser is null)
            return response;

        response.UserName = relatedUser.Name;
        return response;
    }
}

public class RequestToTransactionTransformation
    : AbstractTransformation<TransactionRequest, Transaction>, IRequestToTransactionTransformation
{
}