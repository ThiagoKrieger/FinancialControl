using WebApplication1.Models.Enums;

namespace WebApplication1.Models;

public class TransactionViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public float Value { get; set; }
    public TransactionType Type { get; set; }
    public int UserId { get; set; }
}