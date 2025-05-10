using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetShop.Auth;
using PetShop.Domain.Entities;
using PetShop.Domain.Interfaces;
using PetShop.DTOs;

namespace PetShop.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _userService;

    public AuthController(ILogger<AuthController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequestDTO loginRequest)
    {
        try
        {
            _logger.LogInformation("Login Requested");
            
            var user = await _userService.Authorize(loginRequest.Username, loginRequest.Password);
            
            _logger.LogTrace("Login success for user {user}", user.Username);
        
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Role, "User")
            };
            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)), // время действия 30 минуты
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            return new JsonResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login Failed");
            return Unauthorized();
        }
    }
    
    [HttpPost("Registration")]
    public async Task<ActionResult<string>> Registration([FromBody] RegistrationRequestDTO request)
    {
        try
        { 
            var userToCreate = new User()
            {
                Password = request.Password,
                Username = request.Username,
                email = request.email,
                Role = 1,
                DateOfBirth = request.DateOfBirth,
                Photo = null
            };
            
            await _userService.CreateUser(userToCreate);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return Unauthorized();
        }
    }
}