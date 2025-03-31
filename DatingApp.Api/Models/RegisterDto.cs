using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Models;

public record RegisterDto
{
    [Required]
    public required string UserName { get; init; }
    
    [Required]
    public required string Password { get; init; }    
}