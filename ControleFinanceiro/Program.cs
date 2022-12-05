using ControleFinanceiro.Repository.BusinessLogic;
using ControleFinanceiro.Repository.DBContext;
using ControleFinanceiro.Repository.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<FinancialControlContext>();

builder.Services.AddDbContext<FinancialControlContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IUserDataProvider, UserDataProvider>();
builder.Services.AddTransient<IUserBusinessLogic, UserBusinessLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(name: "default",
    pattern: "{controller=Transactions}/{action=Index}/{id?}");


app.Run();