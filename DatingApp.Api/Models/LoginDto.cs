using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Models;

public record LoginDto
{
    [Required]
    public required string UserName { get; init; }

    [Required]
    public required string Password { get; init; }    
}