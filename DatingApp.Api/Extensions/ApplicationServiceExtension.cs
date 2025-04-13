using System.Text;
using DatingApp.Api.Data;
using DatingApp.Api.Helpers;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Api.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt => {
                var tokenKey = configuration["Security:TokenKey"] ?? throw new KeyNotFoundException("Token key not found");
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false, // no issuer in token
                    ValidateAudience = false // no audience in token
                };
            });

        services.AddDbContext<DataContext>(opt => 
            opt.UseSqlite(configuration.GetConnectionString("DatabaseConnection"))
        );
        return services;
    }
}