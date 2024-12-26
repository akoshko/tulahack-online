using Tulahack.Dtos;

namespace Tulahack.UI.Utils;

public interface IAuthContextProvider
{
    public string? GetAccessToken();
    public string? GetGroup();
    public ContestRoleDto GetRole();
    public PersonBaseDto GetDefaultAccount();
}