using System.Reflection;
using ControleFinanceiro.Repository.BusinessLogic;
using ControleFinanceiro.Repository.DBContext;
using ControleFinanceiro.Repository.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository.Abstractions;
using WebApplication1.Contracts.Transformations;
using WebApplication1.Installers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddValidatorsFromAssemblyContaining<FinancialControlContext>();

builder.Services.AddDbContext<FinancialControlContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IUserDataProvider, UserDataProvider>();
builder.Services.AddTransient<IUserBusinessLogic, UserBusinessLogic>();
builder.Services.AddTransient<IUserToResponseTransformation, UserToResponseToResponseTransformation>();
builder.Services.AddTransient<IRequestToUserTransformation, RequestToUserTransformation>();
builder.Services.AddTransient<IRequestToTransactionTransformation, RequestToTransactionTransformation>();
builder.Services.AddTransient<ITransactionToResponseTransformation, TransactionToResponseTransformation>();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo{Title = "Controle Financeiro", Version = "v1"});
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});
var swaggerOptions = new SwaggerOption();
builder.Configuration.GetSection(nameof(SwaggerOption)).Bind(swaggerOptions);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseSwagger(option =>
{
    option.RouteTemplate = swaggerOptions.JsonRoute;
    option.SerializeAsV2 = true;
});
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
    option.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");   
});


app.Run();