using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Domain.Models.Enums;

public enum TransactionType
{
    [Display(Name = "Income")]
    Income,
    [Display(Name = "Outcome")]
    Outcome
}