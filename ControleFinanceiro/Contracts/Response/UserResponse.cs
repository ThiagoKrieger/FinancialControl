using ControleFinanceiro.Domain.Models;

namespace WebApplication1.Contracts.Response;

public class UserResponse
{
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Balance { get; set; }
    public float Income { get; set; }
    public float Outcome { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}