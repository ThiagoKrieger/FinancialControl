using ControleFinanceiro.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Repository.DBContext;

public class FinancialControlContext : DbContext
{
    public DbSet<UserViewModel> UserViewModels { get; set; }
    public DbSet<TransactionViewModel> TransactionViewModels { get; set; }

    public FinancialControlContext(DbContextOptions<FinancialControlContext> optionsBuilder)
        : base(optionsBuilder)
    {
    }
}