using Domain.Abstractions;

namespace ControleFinanceiro.Domain.Models;

public class User : IEntity
{
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Balance { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}