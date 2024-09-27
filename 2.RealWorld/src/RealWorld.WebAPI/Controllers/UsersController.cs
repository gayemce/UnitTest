using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealWorld.WebAPI.Dtos;
using RealWorld.WebAPI.Services;

namespace RealWorld.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public sealed class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await userService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto request, CancellationToken cancellationToken)
    {
        var result = await userService.CreateAsync(request, cancellationToken);
        if(result)
        {
            return Ok(new {Message = "User registration successful."});
        }
        return BadRequest(new {Message = "An error was encountered during user registration."});
    }

    [HttpGet]
    public async Task<IActionResult> DeleteById(int id, CancellationToken cancellationToken)
    {
        var result = await userService.DeleteByIdAsync(id, cancellationToken);
        if (result)
        {
            return Ok(new { Message = "User deleted successfully."});
        }

        return BadRequest(new { Message = "An error occurred while deleting the user."});
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateUserDto request, CancellationToken cancellationToken)
    {
        var result = await userService.UpdateAsync(request, cancellationToken);
        if (result)
        {
            return Ok(new { message = "User update process successful."});
        }

        return BadRequest(new { Message = "An error occurred during the user update process."});
    }
}
