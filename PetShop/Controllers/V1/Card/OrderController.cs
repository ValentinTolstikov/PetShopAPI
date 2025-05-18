using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Card;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IHttpContextAccessor _currentContext;
    private readonly PetShopContext _context;
    
    public OrderController(IHttpContextAccessor currentContext,
        PetShopContext context)
    {
        _currentContext = currentContext;
        _context = context;
    }
    
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

    [HttpGet]
    public async Task<ActionResult<string>> GetUserOrders()
    {
        var username = _currentContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        var user = await _context.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if(user is null)
            return NotFound($"No tag found with name");

        var userOrders = _context.Transaction.Where(t=>t.IdUser == user.Id).ToList();

        var orders = new List<OrderDTO>();
        
        foreach (var order in userOrders)
        {
            var orderDTO = new OrderDTO()
            {
                idOrder = order.Id,
                products = []
            };
            
            orders.Add(orderDTO);
            
            var products = _context.ProductInTransaction.Where(pt=>pt.IdTransaction == order.Id)
                .ToList();

            foreach (var product in products)
            {
                var prod = _context.Product.FirstOrDefault(p => p.Id == product.Id);
                orderDTO.products.Add(new Tuple<Product, int>(prod,product.SalingCount));
            }
        }
        
        return Ok(orders);
    }

    private class OrderDTO()
    {
        public int idOrder { get; set; }
        public List<Tuple<Product,int>> products { get; set; }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task ChangeOrderStatus(int orderId)
    {
        
    }
}