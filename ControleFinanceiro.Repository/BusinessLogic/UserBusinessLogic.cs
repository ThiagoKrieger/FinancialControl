using ControleFinanceiro.Domain.Models.Enums;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.BusinessLogic;

public class UserBusinessLogic : IUserBusinessLogic
{
    private readonly IUserRepository _userRepository;

    public UserBusinessLogic(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Tuple<float, float>> GetIncomeAndOutcome(int userId, CancellationToken token)
    {
        var user = await _userRepository.GetUserWithTransactions(userId, token).ConfigureAwait(false);
        float incomes = 0;
        float outcomes = 0;

        if (user.Transactions == null)
            return new Tuple<float, float>(incomes, outcomes);
        foreach (var transaction in user.Transactions)
        {
            switch (transaction.Type)
            {
                case TransactionType.Income:
                    incomes += transaction.Value;
                    break;
                case TransactionType.Outcome:
                    outcomes += transaction.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"There is no such type as {transaction.Type}");
            }
        }
        return new Tuple<float, float>(incomes, outcomes);
    }

    public async Task SetBalances(int id, CancellationToken token)
    {
        var user = await _userRepository.GetUserWithTransactions(id, token);

        if (user.Transactions is null || user.Transactions.Count == 0)
        {
            user.Balance = 0;
            await _userRepository.UpdateAsync(user, token);
            return;
        }

        float balance = 0;
        foreach (var transaction in user.Transactions)
        {
            switch (transaction.Type)
            {
                case TransactionType.Income:
                    balance += transaction.Value;
                    break;
                case TransactionType.Outcome:
                    balance -= transaction.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"There is no such Transaction {transaction.Type}");
            }
        }

        user.Balance = balance;
        await _userRepository.UpdateAsync(user, token);
    }
}