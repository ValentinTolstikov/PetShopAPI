using System.Net;
using Microsoft.EntityFrameworkCore;
using PetShop;
using PetShop.Infrastructure.DB;

namespace RecomendationsService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        var services = builder.Services;
        
        var dbconfig = builder.Configuration.GetSection("DbConfiguration");
        
        services.Configure<DbConfiguration>(dbconfig);
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMemoryCache();
        
        var conStr = dbconfig.GetSection("DevConnectionString").Value;

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
        {
            var logger = new LoggerFactory().CreateLogger("Development");
            logger.LogInformation("ASPNETCORE_ENVIRONMENT is not development");
            conStr = dbconfig.GetSection("ProdConnectionString").Value;
        }
        
        services.AddDbContext<PetShopContext>(opt =>
        {
            opt.UseMySql(
                conStr,
                ServerVersion.AutoDetect(conStr),
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}