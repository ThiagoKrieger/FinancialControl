namespace Repository.Abstractions;

public interface IUserBusinessLogic
{
    Task SetBalances(int id, CancellationToken token);
    Task<Tuple<float, float>> GetIncomeAndOutcome(int userId, CancellationToken token);
}