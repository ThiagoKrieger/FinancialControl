using ControleFinanceiro.Domain.Models.Enums;

namespace WebApplication1.Contracts.Request;

public class TransactionRequest
{
    public string Description { get; set; } = string.Empty;
    public float Value { get; set; }
    public TransactionType Type { get; set; }
    public int UserId { get; set; }
}