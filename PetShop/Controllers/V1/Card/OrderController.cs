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

    public class OrderRequest()
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> MakeOrder([FromBody]ICollection<OrderRequest> productsWithCounts)
    {
        var username = _currentContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        var user = await _context.User.FirstOrDefaultAsync(p=>p.Username == username);
        
        if(user is null)
            return NotFound($"No tag found with name");

        var order = new Transaction()
        {
            IdUser = user.Id,
            OrderDate = DateTime.Now,
            IsDeleted = false,
            IsDeliver = false
        };
        
        _context.Transaction.Add(order);
        await _context.SaveChangesAsync();
        
        foreach (var prod in productsWithCounts)
        {
            var fProd = await _context.Product.FirstOrDefaultAsync(p=>p.Id==prod.ProductId);
            
            if (fProd is null)
                return NotFound($"No product found with id {prod.ProductId}");
            
            if (fProd.CountInStock<prod.Count)
                return NotFound($"Product {prod.ProductId} is out of stock");
                
            _context.ProductInTransaction.Add(new ProductInTransaction()
            {
                IdTransaction = order.Id,
                SalingCount = prod.Count,
                IdProduct = prod.ProductId,
                ProductSalingPrice = fProd.Price
            });
        }
        
        await _context.SaveChangesAsync();
        return Ok(order.Id);
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

        var userOrders = _context.Transaction.Where(t=>t.IdUser == user.Id && t.IsDeleted == false).ToList();

        var orders = new List<OrderDTO>();
        
        foreach (var order in userOrders)
        {
            var orderDTO = new OrderDTO()
            {
                idOrder = order.Id,
                isDelivered = order.IsDeliver,
                products = [],
                deliverDate = order.DeliverDate,
                orderDate = order.OrderDate,
            };
            
            orders.Add(orderDTO);
            
            var products = _context.ProductInTransaction.Where(pt=>pt.IdTransaction == order.Id)
                .ToList();

            foreach (var product in products)
            {
                var prod = _context.Product.FirstOrDefault(p => p.Id == product.IdProduct);
                orderDTO.products.Add(new Tuple<Product, int>(prod,product.SalingCount));
            }
        }
        
        return Ok(orders);
    }

    private class OrderDTO
    {
        public int idOrder { get; set; }
        public bool isDelivered { get; set; }
        public DateTime? deliverDate { get; set; }
        public DateTime orderDate { get; set; }
        public List<Tuple<Product,int>> products { get; set; }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task ChangeOrderStatus(int orderId)
    {
        
    }
}