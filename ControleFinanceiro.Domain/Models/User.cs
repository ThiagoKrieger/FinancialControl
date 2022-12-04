using Domain.Abstractions;

namespace ControleFinanceiro.Domain.Models;

public class User : IEntity
{
    public User()
    {
        Name = string.Empty;
    }
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public float Balance { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}