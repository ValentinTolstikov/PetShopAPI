using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.Domain.Entities;

namespace PetShop.Controllers.V1.Card;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class OrderController : ControllerBase
{
    [HttpPost]
    public async Task MakeOrder(List<Product> productsToOrder)
    {
        
    }
    
    [HttpPost]
    public async Task CancelOrder(int orderId)
    {
        
    }

    [HttpGet]
    public async Task CanCancelOrder(int orderId)
    {
        
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task ChangeOrderStatus(int orderId)
    {
        
    }
}