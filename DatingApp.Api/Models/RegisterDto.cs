using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Models;

public record RegisterDto
{
    [Required]
    public string UserName { get; init; } = string.Empty;
    
    [Required]
    [StringLength(8, MinimumLength = 6)]
    public string Password { get; init; } = string.Empty;
}