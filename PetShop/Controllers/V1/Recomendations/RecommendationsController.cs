using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PetShop.Infrastructure.DB;

namespace PetShop.Controllers.V1.Recomendations;

[ApiController]
[Route("[controller]/[action]")]
public class RecommendationsController: ControllerBase
{
    private readonly ILogger<RecommendationsController> _logger;
    private readonly PetShopContext _context;
    private readonly IMemoryCache _cache;

    public RecommendationsController(ILogger<RecommendationsController> logger, 
        PetShopContext context,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _context = context;
        _cache = memoryCache;
    }
    
    [HttpGet]
    public async Task<List<int>> GetRecommendations(int count, int userId = -1)
    {
        _logger.LogInformation("Get recommendations called");
        
        if (userId != -1)
        {
            _logger.LogInformation("Using person id");
        }
        
        var coldStart = !await _context.Transaction.AnyAsync(t=>t.IdUser == userId);


        if (coldStart)
        {
            _logger.LogInformation("Cold start recommendation");

            var items = await GetTopSellingItems(count);

            return items;
        }
        else
        {
            _logger.LogInformation("Hot start recommendation");
            
            var items = await GetUserRecommendations(userId, count);

            return items;
        }
    }

    private async Task<List<int>> GetUserRecommendations(int personId, int count)
    {
        if (_cache.TryGetValue("recs_" + personId + count, out var recs))
        {
            _logger.LogInformation("Get recommendations from cache");
                
            var recommendationsFromCache = (List<int>)recs!;
            return recommendationsFromCache;
        }
        
        //Select 5 latest user transactions
        var userTransactions = await _context.Transaction
            .Where(t => t.IdUser == personId)
            .OrderBy(t=>t.OrderDate)
            .Select(t=>t.Id)
            .Take(5)
            .ToListAsync();

        var productsInTransactionsIds = _context.ProductInTransaction.Where(pt => userTransactions.Contains(pt.IdTransaction))
            .Select(pt=>pt.IdProduct)
            .Distinct();

        var manufacturersIds = _context.Product.Where(p => productsInTransactionsIds.Contains(p.Id))
            .Select(p=>p.IdManufacturer)
            .Distinct();
        
        var categoriesIds = _context.Product.Where(p => productsInTransactionsIds.Contains(p.Id))
            .Select(p=>p.IdCategory)
            .Distinct();
        
        var productsIdsToRecommend = new List<int>();
        
        var similarManufacturersCategoriesProducts = _context.Product.Where(p=>manufacturersIds.Contains(p.IdManufacturer) && categoriesIds.Contains(p.IdCategory))
            .Select(p=>p.Id)
            .Take(count);
        productsIdsToRecommend.AddRange(similarManufacturersCategoriesProducts);

        if (productsIdsToRecommend.Count == count)
        {
            _cache.Set("recs_" + personId + count, productsIdsToRecommend, TimeSpan.FromMinutes(5));
            return productsIdsToRecommend;
        }
            

        var similarCategoriesProducts = _context.Product.Where(p => categoriesIds.Contains(p.IdCategory) && !productsIdsToRecommend.Contains(p.Id))
            .Select(p=>p.Id)
            .Take(count-productsIdsToRecommend.Count);
        productsIdsToRecommend.AddRange(similarCategoriesProducts);

        if (productsIdsToRecommend.Count == count)
        {
            _cache.Set("recs_" + personId + count, productsIdsToRecommend, TimeSpan.FromMinutes(5));
            return productsIdsToRecommend;
        }
            
        
        var similarManufacturerProducts = _context.Product.Where(p => manufacturersIds.Contains(p.IdManufacturer) && !productsIdsToRecommend.Contains(p.Id))
            .Select(p=>p.Id)
            .Take(count-productsIdsToRecommend.Count);
        productsIdsToRecommend.AddRange(similarManufacturerProducts);

        if (productsIdsToRecommend.Count == count)
        {
            _cache.Set("recs_" + personId + count, productsIdsToRecommend, TimeSpan.FromMinutes(5));
            return productsIdsToRecommend;
        }
        
        var topSellingItems = _context.Product.Where(p => !productsIdsToRecommend.Contains(p.Id))
            .OrderBy(p=>p.ViewsCount)
            .Take(count-productsIdsToRecommend.Count)
            .Select(p=>p.Id);
        
        productsIdsToRecommend.AddRange(topSellingItems);
        
        _cache.Set("recs_" + personId + count, productsIdsToRecommend, TimeSpan.FromMinutes(5));
        return productsIdsToRecommend;
    }

    private async Task<List<int>> GetTopSellingItems(int count)
    {
        if (_cache.TryGetValue("TopSellingItems"+count, out var topSellingItemsCache))
        {
            _logger.LogInformation("Get top selling items from cache");
            
            var itemsList = (List<int>)topSellingItemsCache!;
            return itemsList;
        }
        
        var topSellingItems = await _context.Product.OrderBy(p=>p.ViewsCount)
            .Take(count)
            .Select(p=>p.Id)
            .ToListAsync();
        
        _cache.Set("TopSellingItems"+count, topSellingItems, new DateTimeOffset(DateTime.Now.AddMinutes(5)));
        return topSellingItems;
    }
}