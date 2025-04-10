using System.Security.Claims;
using AutoMapper;
using DatingApp.Api.Entities;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
    {
        var user = await userRepository.GetMemberAsync(username);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) 
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(username is null)
            return BadRequest("No username found in token");

        var user = await userRepository.GetUserByUsernameAsync(username);
    
        if(user is null)
            return BadRequest("Not found user");

        mapper.Map(memberUpdateDto, user);

        if(await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");    
    }
}