using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Admin;

[ApiController]
[Route("[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly PetShopContext _db;
    private readonly ILogger<AdminController> _logger;
    
    public AdminController(PetShopContext context,
        ILogger<AdminController> logger)
    {
        _db = context;
        _logger = logger;
    }

    public class UserResponseDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
    
    [HttpGet]
    public async Task<ActionResult<UserResponseDTO>> GetUsers()
    {
        _logger.LogInformation("Get users called");

        var response = _db.User.Select(u=> new UserResponseDTO()
        {
            Role = u.Role == 0 ? "Admin" : "User",
            Email = u.email,
            Status = u.IsActive == true ? "Active" : "Inactive",
            Username = u.Username
        });
        
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult> ChangeUser(string username, bool active)
    {
        _logger.LogInformation("Change user status for {user} to {status} called", username, active);

        var user = await _db.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if (user is null)
            return NotFound();
        
        user.IsActive = active;
        await _db.SaveChangesAsync();
        
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductStatisticResponseDTO>>> GetProductsSales()
    {
        _logger.LogInformation("Get products sales called");
        
        var pts = await _db.ProductInTransaction.ToListAsync();
        var pIds = pts.Select(p => p.Id).Distinct().ToList();
        var products = await _db.Product.Where(p=>pIds.Contains(p.Id)).ToListAsync();

        var response = new List<ProductStatisticResponseDTO>();
        
        foreach (var product in products)
        {
            var prodDTO = new ProductStatisticResponseDTO()
            {
                ProductName = product.Title,
                CountInStock = product.CountInStock,
                CountOutOfStock = pts.Where(p=>p.IdProduct == product.Id).Sum(p=>p.SalingCount)
            };
            
            response.Add(prodDTO);
        }
        
        return Ok(response);
    }

    public class ProductStatisticResponseDTO
    {
        public string ProductName { get; set; }
        public int CountInStock { get; set; }
        public int CountOutOfStock { get; set; }
    }
}















