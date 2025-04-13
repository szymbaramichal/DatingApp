using System.Security.Claims;
using AutoMapper;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
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

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if(user is null)
            return BadRequest("Cannot update user");

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error is not null)
            return BadRequest(result.Error.Message);

        var photo = new Photo {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo); 

        if(await userRepository.SaveAllAsync()) 
            return CreatedAtAction(nameof(GetUserByUsername), new { username = user.UserName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) 
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
    
        if(user is null)
            return BadRequest("Not found user");

        mapper.Map(memberUpdateDto, user);

        if(await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");    
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if(user is null)
            return BadRequest("Not found user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if(photo is null)
            return NotFound("Photo not found");

        if(photo.IsMain) 
            return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

        if(currentMain is not null) 
            currentMain.IsMain = false;

        photo.IsMain = true;

        if(await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to set main photo");
    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if(user is null)
            return BadRequest("Not found user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if(photo is null)
            return NotFound("Photo not found");

        if(photo.IsMain) 
            return BadRequest("You cannot delete your main photo");

        if(photo.PublicId is not null) 
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error is not null) 
                return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if(await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to delete the photo");
    }  
}