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
        var photoIds = _context.ProductPhoto.Where(p => p.IdProduct == productId).Select(p=>p.IdPhoto).ToArray();
        var photos = _context.Photo.Where(p => photoIds.Contains(p.Id));
        
        var dtos = new List<PhotoDTO>();

        foreach (var photo in photos)
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