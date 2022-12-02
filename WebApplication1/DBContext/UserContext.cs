using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.DBContext;

public class FinancialControl : DbContext
{
    public DbSet<UserViewModel> UserViewModels { get; set; }
    public DbSet<TransactionViewModel> TransactionViewModels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Password=mssqlserver;Persist Security Info=True;User ID=sa;Initial Catalog=ControleFinanceiro;Data Source=THIAGO; Encrypt=False;");

    }
}