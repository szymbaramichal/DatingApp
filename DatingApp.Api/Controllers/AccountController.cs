using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.UserName))
            return BadRequest("UserName is already taken.");

        return Ok();

        // using var hmac = new HMACSHA256();

        // var appUser = new AppUser 
        // {
        //     UserName = registerDto.UserName.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        //     PasswordSalt = hmac.Key
        // };
        
        // await context.AppUsers.AddAsync(appUser);
        // await context.SaveChangesAsync();

        // return new UserDto {
        //     Username = appUser.UserName,
        //     Token = tokenService.CreateToken(appUser),
        // };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.AppUsers.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if(user is null)
            return BadRequest("Invalid username or password");

        using var hmac = new HMACSHA256(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i = 0; i < computedHash.Length; i++)
        {
            if(computedHash[i] != user.PasswordHash[i])
                return BadRequest("Invalid username or password");
        }

        return new UserDto {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
        };
    }



    private async Task<bool> UserExists(string userName)
    {
        return await context.AppUsers.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
    }
}