using System.Security.Claims;

namespace DatingApp.Api.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Username not found");
        
        return username;
    }
}