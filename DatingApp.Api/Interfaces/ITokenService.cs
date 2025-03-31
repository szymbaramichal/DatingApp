using DatingApp.Api.Entities;

namespace DatingApp.Api.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}