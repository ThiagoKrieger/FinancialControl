using ControleFinanceiro.Domain.Models;
using FluentValidation;

namespace ControleFinanceiro.Repository.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Age).Must(age => age >= 0);
        RuleFor(user => user.Name).NotEmpty();
    }
}