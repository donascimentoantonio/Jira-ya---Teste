using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _jwtAuthService;

    public AuthController(IAuthenticationService jwtAuthService)
    {
        _jwtAuthService = jwtAuthService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _jwtAuthService.AuthenticateAsync(request.Username, "");
        if (token != null)
        {
            return Ok(new { token });
        }
        return Unauthorized();
    }
}


