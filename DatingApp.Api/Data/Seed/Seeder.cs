using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace DatingApp.Api.Data.Seed
{
    public class Seeder
    {
        public static async Task SeedUsers(DataContext context)
        {
            if(await context.AppUsers.AnyAsync())
                return;

            var userData = await File.ReadAllTextAsync("Data/Seed/UserSeedData.json");

            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
            };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            foreach(var user in users)
            {
                using var hmac = new HMACSHA256();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Test123!"));
                user.PasswordSalt = hmac.Key;

                context.AppUsers.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}