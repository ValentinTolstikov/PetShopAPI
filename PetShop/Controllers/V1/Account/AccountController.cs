using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Domain.Entities;
using PetShop.DTOs;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Account;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IHttpContextAccessor _currentContext;
    private readonly PetShopContext _dbContext;
    private ILogger<AccountController> _logger;
    
    public AccountController(IHttpContextAccessor currentContext, PetShopContext dbContext, ILogger<AccountController> logger)
    {
        _currentContext = currentContext;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<UserInfoResponseDto>> GetUserInfo()
    {
        _logger.LogInformation("Get user info required");
        
        var username = _currentContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        var user = await _dbContext.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if(user is null)
            return NotFound($"No tag found with name");
        
        var base64 = Encoding.UTF8.GetString(user.Photo);
        
        return Ok(new UserInfoResponseDto(user.Username, user.email, user.DateOfBirth.ToString("dd-MM-yyyy"), base64));
    }

    [HttpGet]
    public async Task<ActionResult<UserAdress>> GetUserAddress()
    {
        var username = _currentContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        var user = await _dbContext.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if(user is null)
            return NotFound($"No tag found with name");
        
        return _dbContext.UserAdress.FirstOrDefault(a=>a.IdUser == user.Id);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateUserAddress([FromBody] UserAdress userAddress)
    {
        var username = _currentContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        var user = await _dbContext.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if(user is null)
            return NotFound($"No tag found with name");
        
        var isExist = await _dbContext.UserAdress.AnyAsync(a=>a.IdUser == user.Id);

        if (isExist)
        {
            _dbContext.UserAdress.Update(userAddress);
        }
        else
        {
            _dbContext.UserAdress.Add(userAddress);
        }
        
        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }
}