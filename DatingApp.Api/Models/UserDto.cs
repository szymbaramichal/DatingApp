namespace DatingApp.Api.Models;

public record UserDto
{
    public required string Username { get; init; }
    public required string Token { get; init; }       
    public string? PhotoUrl { get; set; }
}