using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PetShop.Domain.Entities;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Image;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class ImageController
{
    private readonly PetShopContext _context;
    private readonly IMemoryCache _cache;
    
    public ImageController(PetShopContext context, ILogger<ImageController> logger,
        IMemoryCache memoryCache)
    {
        _context = context;
        _cache = memoryCache;
    }
    
    [HttpGet]
    public async Task<IEnumerable<PhotoDTO>> Product(int productId)
    {
        var photos = Array.Empty<Photo>();
            
        if (_cache.TryGetValue("Photos"+productId, out var cachePhotos))
        {
            photos = cachePhotos as Photo[];
        }
        else
        {
            var photoIds = _context.ProductPhoto.Where(p => p.IdProduct == productId).Select(p=>p.IdPhoto).ToArray();
            photos = _context.Photo.Where(p => photoIds.Contains(p.Id)).ToArray();
            _cache.Set("Photos"+productId, photos, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            });
        }
        
        
        var dtos = new List<PhotoDTO>();

        if (photos != null)
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