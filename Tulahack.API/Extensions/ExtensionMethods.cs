using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tulahack.API.Static;
using Tulahack.Model;

namespace Tulahack.API.Extensions;

public static class ExtensionMethods
{
    public static Guid GetUserId(this ClaimsPrincipal claims)
    {
        var ssoUserClaim = claims.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        var result = Guid.TryParse(ssoUserClaim.Value, out var userId);
        
        if (result)
            return userId;

        return default;
    }
    
    public static ContestRole GetRole(this string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var claims = jwt.Claims
            .Where(claim => claim.Type == "group")
            .Select(claim => claim.Value)
            .ToList();
        
        if (claims.Count == 0)
            return ContestRole.Visitor;
        
        // checking group membership from strongest to weakest
        if (claims.Contains(Groups.Superuser, StringComparer.InvariantCultureIgnoreCase))
            return ContestRole.Superuser;
        if (claims.Contains(Groups.Moderator, StringComparer.InvariantCultureIgnoreCase))
            return ContestRole.Moderator;
        if (claims.Contains(Groups.Expert, StringComparer.InvariantCultureIgnoreCase))
            return ContestRole.Expert;
        if (claims.Contains(Groups.Contestant, StringComparer.InvariantCultureIgnoreCase))
            return ContestRole.Contestant;

        return ContestRole.Visitor;
    }
    
    public static string GetStoragePath(this Guid id, ContestRole role)
    {
        switch (role)
        {
            case ContestRole.Contestant:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}");
            case ContestRole.Expert:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}");
            case ContestRole.Moderator:
            case ContestRole.Superuser:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}");
            case ContestRole.Visitor:
            default:
                throw new ArgumentOutOfRangeException(nameof(role), role, null);
        }
    }
    
    public static string GetStoragePath(this Guid id, ContestRole role, FilePurposeType purpose, string filename)
    {
        switch (role)
        {
            case ContestRole.Contestant:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}", $"{purpose.ToString()}", filename);
            case ContestRole.Expert:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}", $"{purpose.ToString()}", filename);
            case ContestRole.Moderator:
            case ContestRole.Superuser:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}", $"{purpose.ToString()}", filename);
            case ContestRole.Visitor:
            default:
                return string.Empty;
        }
    }
    
    public static string GetStoragePath(this Guid id, ContestRole role, FilePurposeType purpose)
    {
        switch (role)
        {
            case ContestRole.Contestant:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}", $"{purpose.ToString()}");
            case ContestRole.Expert:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}", $"{purpose.ToString()}");
            case ContestRole.Moderator:
            case ContestRole.Superuser:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}", $"{purpose.ToString()}");
            case ContestRole.Visitor:
            default:
                return string.Empty;
        }
    }
    
    public static string GetStoragePath(this Guid id, ContestRole role, string filename)
    {
        switch (role)
        {
            case ContestRole.Contestant:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"team_{id}", filename);
            case ContestRole.Expert:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"company_{id}", filename);
            case ContestRole.Moderator:
            case ContestRole.Superuser:
                return Path.Combine(Directory.GetCurrentDirectory(), "appData", "storage", $"static_{id}", filename);
            case ContestRole.Visitor:
            default:
                return string.Empty;
        }
    }
}