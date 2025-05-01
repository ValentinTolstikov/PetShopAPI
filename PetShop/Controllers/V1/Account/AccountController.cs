using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetShop.Controllers.V1.Account;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class AccountController
{
    [HttpGet]
    public async Task GetUserInfo(int userId)
    {
        
    }
}