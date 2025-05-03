using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Image;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class ImageController
{
    private readonly PetShopContext _context;
    
    public ImageController(PetShopContext context, ILogger<ImageController> logger)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Photo>> Product(int productId)
    {
        return _context.ProductPhoto.Where(p=>p.IdProduct == productId).Include(p=>p.Photo).Select(p=>p.Photo);
    }
}