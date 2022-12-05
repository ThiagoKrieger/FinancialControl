using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Domain.Models.Enums;
using FluentValidation;
using Repository.Abstractions;

namespace ControleFinanceiro.Repository.Validators;

public class TransactionValidator : AbstractValidator<Transaction>
{
    public TransactionValidator(IUserRepository businessLogic)
    {
        RuleFor(transaction => transaction.Description)
            .NotEmpty()
            .WithMessage("The description can't be null");
        
        RuleFor(transaction => transaction.Value)
            .Must(value => value > 0)
            .WithMessage("The value must be positive");
        
        RuleFor(transaction => transaction.Type)
            .MustAsync(async (transaction, type, token) =>
            {
                if (type == TransactionType.Outcome)
                    return true;
                return (await businessLogic.GetAsync(token)).Any(user =>
                        user.Id == transaction.UserId && user.Age >= 18);
            }).WithMessage("The user must be over age in order to have incomes. Change the transaction type or select and over age user.");
    }
}