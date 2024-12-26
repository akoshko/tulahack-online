using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Tulahack.Dtos;
using Tulahack.UI.Constants;

namespace Tulahack.UI.Utils;

public abstract class AbstractAuthContextProvider : IAuthContextProvider
{
    public abstract string? GetAccessToken();
    public abstract string? GetGroup();

    public ContestRoleDto GetRole()
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(GetAccessToken());
        var claims = token.Claims
            .Where(claim => claim.Type == "group")
            .Select(claim => claim.Value)
            .ToList();
        
        if (claims.Count == 0)
            return ContestRoleDto.Visitor;
        
        // checking group membership from strongest to weakest
        if (claims.Contains(Groups.Superuser, StringComparer.InvariantCultureIgnoreCase))
            return ContestRoleDto.Superuser;
        if (claims.Contains(Groups.Moderator, StringComparer.InvariantCultureIgnoreCase))
            return ContestRoleDto.Moderator;
        if (claims.Contains(Groups.Expert, StringComparer.InvariantCultureIgnoreCase))
            return ContestRoleDto.Expert;
        if (claims.Contains(Groups.Contestant, StringComparer.InvariantCultureIgnoreCase))
            return ContestRoleDto.Contestant;

        return ContestRoleDto.Visitor;
    }

    public PersonBaseDto GetDefaultAccount()
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(GetAccessToken());
        
        var id = Guid.Parse(token.Claims
            .Where(claim => claim.Type == "sub")
            .Select(claim => claim.Value)
            .FirstOrDefault() ?? Guid.Empty.ToString());
        
        return new PersonBaseDto()
        {
            Id = id,
            Firstname = token.Claims
                .Where(claim => claim.Type == "given_name")
                .Select(claim => claim.Value)
                .FirstOrDefault() ?? string.Empty,
            Lastname = token.Claims
                .Where(claim => claim.Type == "family_name")
                .Select(claim => claim.Value)
                .FirstOrDefault() ?? string.Empty,
            Email = token.Claims
                .Where(claim => claim.Type == "email")
                .Select(claim => claim.Value)
                .FirstOrDefault() ?? string.Empty,
            Role = ContestRoleDto.Visitor,
        };
    }

}