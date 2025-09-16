using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        try
        {
            var user = await userService.SignUpAsync(request.Email, request.Password, request.FullName);
            return Ok(new { user.Id, user.Email, user.FullName });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var user = await userService.SignInAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized();

        return Ok(new { user.Id, user.Email, user.FullName });
    }
}