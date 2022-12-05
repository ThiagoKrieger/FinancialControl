using ControleFinanceiro.Domain.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;
using WebApplication1.Contracts.Request;
using WebApplication1.Contracts.Transformations;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly IUserRepository _repository;
    private readonly IValidator<User> _validator;
    private readonly IRequestToUserTransformation _fromRequestTransformation;
    private readonly IUserToResponseTransformation _toResponseToResponseTransformation;

    public UserController(IUserRepository userRepository,
        IValidator<User> validator,
        IRequestToUserTransformation fromRequestTransformation,
        IUserToResponseTransformation toResponseToResponseTransformation
        )
    {
        _repository = userRepository;
        _validator = validator;
        _fromRequestTransformation = fromRequestTransformation;
        _toResponseToResponseTransformation = toResponseToResponseTransformation;
    }

    /// <summary>
    /// This returns all the existing users
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userList = (await _repository.GetAllUsersWithTransactions(cancellationToken)).ToList();
        var response = await _toResponseToResponseTransformation.TransformToMany(userList, cancellationToken); 

        return Ok(response);
    }
    /// <summary>
    /// This returns an specific user
    /// </summary>
    [HttpGet("details")]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserWithTransactions(id, cancellationToken);
        var response = await _toResponseToResponseTransformation.TransformTo(user, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// This creates an user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(UserRequest userRequest,
        CancellationToken cancellationToken)
    {
        var user = await _fromRequestTransformation.TransformTo(userRequest, cancellationToken);
        var result = await _validator.ValidateAsync(user, cancellationToken);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        if (!await _repository.AddAsync(user, cancellationToken))
            return Problem($"Wasn't able to save {userRequest.Name}");

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// This edits an existing user
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Edit(int id,
        UserRequest userRequest,
        CancellationToken cancellationToken)
    {
        var userToUpdate = await _repository.GetByKeyAsync(id, cancellationToken);
        if (userToUpdate is null)
            return NotFound();
        
        var userFromRequest = await _fromRequestTransformation.TransformTo(userRequest, userToUpdate, cancellationToken);
        
        var result = await _validator.ValidateAsync(userFromRequest, cancellationToken);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        try
        {
            await _repository.UpdateAsync(userFromRequest, cancellationToken);
        }
        catch (DbUpdateConcurrencyException e)
        {
            return NotFound(e.Message);
        }

        var updatedUser = await _repository.GetUserWithTransactions(id, cancellationToken);
        return Ok(updatedUser);
    }

    /// <summary>
    /// This deletes an existing user
    /// </summary>
    /// <returns>All existing users</returns>
    /// <returns>NotFound</returns>
    [HttpDelete, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        return await _repository.RemoveAsync(id, cancellationToken)
            ? RedirectToAction(nameof(Index))
            : NotFound("There is no such entity in database to be deleted");
    }
}