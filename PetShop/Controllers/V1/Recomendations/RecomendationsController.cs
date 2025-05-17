using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Recomendations;

[ApiController]
[Route("[controller]/[action]")]
public class RecomendationsController:ControllerBase
{
    private readonly IHttpContextAccessor _currentContext;
    private readonly PetShopContext _context;
    
    public RecomendationsController(IHttpContextAccessor currentContext,
        PetShopContext context)
    {
        _currentContext = currentContext;
        _context = context;
    }
    
    [HttpGet(Name = "Ping")]
    public async Task<string> GetPing()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync("http://recomendationsservice:25000/ping");
        var content = await response.Content.ReadAsStringAsync();
        return content.ToString();
    }
    
    [HttpGet(Name = "Recommendations")]
    public async Task<ActionResult<List<int>>> GetRecommendations(int count)
    {
        var username = _currentContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        var user = await _context.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if(user is null)
            return NotFound($"No tag found with name");
        
        using var client = new HttpClient();
        var response = await client.GetAsync($"http://recomendationsservice:25000/Recommendations/GetRecommendations?count={count}&userId={user.Id}");
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var ids = JsonConvert.DeserializeObject<List<int>>(responseContent);
        
        return ids;
    }
}