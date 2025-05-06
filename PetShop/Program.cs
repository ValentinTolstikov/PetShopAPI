using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetShop.Application;
using PetShop.Auth;
using PetShop.Domain.Interfaces;
using PetShop.Infrastructure.DB;
using PetShop.Infrastructure.DB.Repositories;

namespace PetShop;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        /*builder.WebHost.UseKestrel(options =>
        {
            options.Listen(IPAddress.Parse("127.0.0.1"), 25000);
        });*/
        
        var services = builder.Services;
        
        var dbconfig = builder.Configuration.GetSection("DbConfiguration");
        
        services.Configure<DbConfiguration>(dbconfig);
        services.AddCors();
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
        });

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
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        var app = builder.Build();

        app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build());
        
            app.UseSwagger();
            app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}