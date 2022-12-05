using ControleFinanceiro.Domain.Models.Enums;

namespace WebApplication1.Contracts.Response;

public class TransactionResponse
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public float Value { get; set; }
    public TransactionType Type { get; set; }
    public int UserId { get; set; }
}