using ControleFinanceiro.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Repository.DBContext;

public class FinancialControlContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Transaction> Transaction { get; set; }

    public FinancialControlContext(DbContextOptions<FinancialControlContext> optionsBuilder)
        : base(optionsBuilder)
    {
    }
}