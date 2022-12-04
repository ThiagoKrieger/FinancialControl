using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Views.Transactions;

public class TransactionViewMode
{
    [DisplayName("Transaction")]
    public int Id { get; set; }

    public IList<SelectListItem> UserItems { get; set; }
}