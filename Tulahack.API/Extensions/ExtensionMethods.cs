using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tulahack.API.Static;
using Tulahack.Model;

namespace Tulahack.API.Extensions;

public static class ExtensionMethods
{
    public static Guid GetUserId(this ClaimsPrincipal claims)
    {
        Claim ssoUserClaim = claims.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        var result = Guid.TryParse(ssoUserClaim.Value, out Guid userId);

        return result ? userId : default;
    }

    public static ContestRole GetRole(this string token)
    {
        JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var claims = jwt.Claims
            .Where(claim => claim.Type == "group")
            .Select(claim => claim.Value)
            .ToList();

        if (claims.Count == 0)
        {
            return ContestRole.Visitor;
        }

        // checking group membership from strongest to weakest
        if (claims.Contains(Groups.Superuser, StringComparer.InvariantCultureIgnoreCase))
        {
            return ContestRole.Superuser;
        }

        if (claims.Contains(Groups.Moderator, StringComparer.InvariantCultureIgnoreCase))
        {
            return ContestRole.Moderator;
        }

        if (claims.Contains(Groups.Expert, StringComparer.InvariantCultureIgnoreCase))
        {
            return ContestRole.Expert;
        }

        if (claims.Contains(Groups.Contestant, StringComparer.InvariantCultureIgnoreCase))
        {
            return ContestRole.Contestant;
        }

        return ContestRole.Visitor;
    }

    public static string GetStoragePath(this Guid id, ContestRole role) =>
        role switch
        {
            ContestRole.Contestant => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}"),
            ContestRole.Expert => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}"),
            ContestRole.Moderator or ContestRole.Superuser => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}"),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null),
        };

    public static string GetStoragePath(this Guid id, ContestRole role, FilePurposeType purpose, string filename) =>
        role switch
        {
            ContestRole.Contestant => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}",
                $"{purpose.ToString()}", filename),
            ContestRole.Expert => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}",
                $"{purpose.ToString()}", filename),
            ContestRole.Moderator or ContestRole.Superuser => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}",
                $"{purpose.ToString()}", filename),
            _ => string.Empty,
        };

    public static string GetStoragePath(this Guid id, ContestRole role, FilePurposeType purpose) =>
        role switch
        {
            ContestRole.Contestant => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}",
                $"{purpose.ToString()}"),
            ContestRole.Expert => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}",
                $"{purpose.ToString()}"),
            ContestRole.Moderator or ContestRole.Superuser => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}",
                $"{purpose.ToString()}"),
            _ => string.Empty,
        };

    public static string GetStoragePath(this Guid id, ContestRole role, string filename) =>
        role switch
        {
            ContestRole.Contestant => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}", filename),
            ContestRole.Expert => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}", filename),
            ContestRole.Moderator or ContestRole.Superuser => Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}", filename),
            _ => string.Empty,
        };
}
