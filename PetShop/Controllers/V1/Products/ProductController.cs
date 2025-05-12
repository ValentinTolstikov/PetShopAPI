using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Products;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class ProductController : ControllerBase
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
    public async Task<ActionResult<IEnumerable<Product>>> GetPage(int page = 1, int pageSize = 10, string? tag = null, string? category = null)
    {
        _logger.LogInformation("GetProducts called.");

        var source = _context.Product.AsQueryable();
        
        if (tag is not null)
        {
            var tagId = (await _context.Tag.FirstOrDefaultAsync(t=>t.Name==tag))?.Id;

            if (tagId is null)
                return NotFound($"No tag found with name {tag}");

            var productTags = _context.ProductTag
                .Where(pt => pt.IdTag == tagId)
                .Select(pt => pt.Id)
                .AsQueryable();

            source = source.Where(p => productTags.Contains(p.Id));
        }

        if (category is not null)
        {
            var reqCategory = await _context.Category.FirstOrDefaultAsync(c=>c.Name==category);
            
            if(reqCategory is null)
                return NotFound("Category not found");
            
            source = source.Where(p => p.IdCategory == reqCategory.Id);
        }
        
        return Ok(source.Skip(pageSize * (page - 1)).Take(pageSize));
    }

    [HttpGet]
    public async Task<ActionResult<Product>> Get(int id)
    {
        _logger.LogInformation("GetProduct called.");
        
        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
        
        if (product is null)
            return NotFound("Product not found");
        
        product.ViewsCount += 1;
        await _context.SaveChangesAsync();
        
        return Ok(product);
    }

    [HttpPut]
    public async Task Update(Product product)
    {
        _logger.LogInformation("UpdateProduct called.");
        
        _context.Product.Update(product);
        await _context.SaveChangesAsync();
    }

    [HttpPost]
    public async Task Create(Product product)
    {
        _logger.LogInformation("CreateProduct called.");
        
        _context.Product.Add(product);
        await _context.SaveChangesAsync();
    }

    [HttpDelete]
    public async Task<ActionResult<int>> Delete(int id)
    {
        _logger.LogInformation("DeleteProduct called.");
        
        var product = _context.Product.FirstOrDefault(p => p.Id == id);
        
        if (product is null)
            return NotFound($"No product found with name {id}");
        
        product.CountInStock = 0;
        await _context.SaveChangesAsync();
        return product.Id;
    }
}