namespace WebApplication1.Models;

public class UserViewModel
{
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public float Balance { get; set; }
    public ICollection<TransactionViewModel> TransactionViewModels { get; set; }
}