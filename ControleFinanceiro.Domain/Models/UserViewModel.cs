using Domain.Abstractions;

namespace ControleFinanceiro.Domain.Models;

public class UserViewModel : IEntity
{
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public float Balance { get; set; }
    public ICollection<TransactionViewModel> TransactionViewModels { get; set; }
}