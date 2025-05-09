using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    
    public AccountController(IHttpContextAccessor currentContext, PetShopContext dbContext)
    {
        _currentContext = currentContext;
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public async Task<ActionResult<UserInfoResponseDto>> GetUserInfo()
    {
        var username = _currentContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        var user = await _dbContext.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if(user is null)
            return NotFound($"No tag found with name");
        
        return Ok(new UserInfoResponseDto(user.Username, user.email, user.DateOfBirth.ToString("dd-MM-yyyy")));
    }
}