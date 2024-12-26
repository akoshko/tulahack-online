using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Tulahack.UI.Constants;

namespace Tulahack.UI.Utils;

public static class TokenExtensions
{
    public static string GetGroup(this string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var claims = jwt.Claims
            .Where(claim => claim.Type == "group")
            .Select(claim => claim.Value)
            .ToList();
        
        if (claims.Count == 0)
            return Groups.Public;
        
        // checking group membership from strongest to weakest
        if (claims.Contains(Groups.Superuser, StringComparer.InvariantCultureIgnoreCase))
            return Groups.Superuser;
        if (claims.Contains(Groups.Moderator, StringComparer.InvariantCultureIgnoreCase))
            return Groups.Moderator;
        if (claims.Contains(Groups.Expert, StringComparer.InvariantCultureIgnoreCase))
            return Groups.Expert;
        if (claims.Contains(Groups.Contestant, StringComparer.InvariantCultureIgnoreCase))
            return Groups.Contestant;

        return Groups.Public;
    }
}