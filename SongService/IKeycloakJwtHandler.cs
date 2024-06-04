using Microsoft.IdentityModel.JsonWebTokens;

namespace SongService;

public interface IKeycloakJwtHandler
{
    Task<bool> IsValidJWT(JsonWebToken token);
    IEnumerable<string> GetRoles(JsonWebToken token);
}