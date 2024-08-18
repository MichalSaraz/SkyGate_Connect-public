using Core.Identity;

namespace Core.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Creates a token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to create the token.</param>
    /// <returns>The generated token as a string.</returns>
    string CreateToken(AppUser user);
}