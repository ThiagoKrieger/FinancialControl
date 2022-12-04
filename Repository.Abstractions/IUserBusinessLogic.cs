namespace Repository.Abstractions;

public interface IUserBusinessLogic
{
    Task SetBalances(int id, CancellationToken token);
}