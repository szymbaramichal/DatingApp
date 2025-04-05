using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers;

public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "text";
    }

    [HttpGet("not-found")]
    public ActionResult<string> GetNotFound()
    {
        var user = context.AppUsers.Find(-1);
        if(user is null) return NotFound();

        return Ok();
    }

    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
        var user = context.AppUsers.Find(-1) ?? throw new Exception("STH_WENT_WRONG");

        return Ok(user);
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was bad request.");
    }
}