using System.Text;
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
    public async Task<IEnumerable<PhotoDTO>> Product(int productId)
    {
        var test = _context.ProductPhoto.Where(p=>p.IdProduct == productId).Include(p=>p.Photo).Select(p=>p.Photo).ToArray();
        
        if(test.Length == 0)
            return Array.Empty<PhotoDTO>();
        
        var dtos = new List<PhotoDTO>();

        foreach (var photo in test)
        {
            var base64 = Encoding.UTF8.GetString(photo.Data);
            var dto = new PhotoDTO()
            {
                Data = base64,
            };
            dtos.Add(dto);
        }
        
        return dtos.ToArray();
    }

    public class PhotoDTO
    {
        public string Data { get; set; }
    }
}