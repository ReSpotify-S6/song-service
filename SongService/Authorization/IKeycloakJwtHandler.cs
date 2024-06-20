using Microsoft.IdentityModel.JsonWebTokens;

namespace SongService.Authorization;

public interface IKeycloakJwtHandler
{
    Task<bool> IsValidJWT(JsonWebToken token);
    IEnumerable<string> GetRoles(JsonWebToken token);
}