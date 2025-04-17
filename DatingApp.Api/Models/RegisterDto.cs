using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Models;

public record RegisterDto
{
    [Required]
    public string UserName { get; init; } = default!;

    [Required]
    public string KnownAs { get; init; } = default!;

    [Required]
    public string Gender { get; init; } = default!;

    [Required]
    public string DateOfBirth { get; init; } = default!;

    [Required]
    public string City { get; init; } = default!;

    [Required]
    public string Country { get; init; } = default!;
    
    [Required]
    [StringLength(8, MinimumLength = 6)]
    public string Password { get; init; } = default!;
}