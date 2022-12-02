using ControleFinanceiro.Domain.Models;
using ControleFinanceiro.Repository.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class UserController : Controller
{
    private readonly FinancialControlContext _context;

    public UserController(FinancialControlContext context)
    {
        _context = context;
    }
    
    // GET : /User/Get
    public IActionResult Get()
    {
        var users = _context.UserViewModels.ToList();
        return Ok(users);
    }
    
    // POST : /User/Add
    public IActionResult Add(string name, int age)
    {
        var user = new UserViewModel
        {
            Name = name,
            Age = age
        };
        
        _context.UserViewModels.Add(user);
        _context.SaveChanges();
        
        return Ok(user);
    }
}