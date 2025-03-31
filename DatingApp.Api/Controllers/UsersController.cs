using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers;

[Authorize]
public class UsersController(DataContext context) : BaseApiController
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ICollection<AppUser>>> GetUsers()
    {
        return Ok(await context.AppUsers.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUsers(int id)
    {
        var user = await context.AppUsers.FindAsync(id);

        if (user is null)
            return NotFound();

        return Ok(user);
    }
}