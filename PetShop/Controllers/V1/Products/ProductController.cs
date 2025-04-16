using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Products;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProductController
{
    private readonly ILogger<ProductController> _logger;
    private readonly PetShopContext _context;
    
    public ProductController(ILogger<ProductController> logger, 
        PetShopContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "Products")]
    public IEnumerable<Product> GetProducts(int page = 1, int pageSize = 10)
    {
        _logger.LogInformation("GetProducts called.");
        return _context.Product.Skip(pageSize * (page-1)).Take(pageSize);
    }
}