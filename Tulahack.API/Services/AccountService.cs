using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Options;
using Tulahack.API.Context;
using Tulahack.API.Extensions;
using Tulahack.API.Utils;
using Tulahack.Dtos;
using Tulahack.Model;

namespace Tulahack.API.Services;

public interface IAccountService
{
    Task<PersonBase?> GetAccount(Guid userId);
    Task<ContestantDto?> GetContestantDetails(Guid userId);
    Task<ExpertDto?> GetExpertDetails(Guid userId);
    Task<ModeratorDto?> GetModeratorDetails(Guid userId);

    Task<PersonBaseDto?> UpdateAccount(PersonBaseDto dto);
    Task<ContestantDto?> UpdateContestant(ContestantDto dto);
    Task<ExpertDto?> UpdateExpert(ExpertDto dto);
    Task<ModeratorDto?> UpdateModerator(ModeratorDto dto);

    Task<PersonBase> CreateAccount(string jwt);
    Task<PersonBaseDto?> CreateAccount(PersonBaseDto dto);
    Task<ContestantDto?> CreateContestant(ContestantDto dto);
    Task<ExpertDto?> CreateExpert(ExpertDto dto);
    Task<ModeratorDto?> CreateModerator(ModeratorDto dto);
    Task<PersonBase?> DeleteAccount(Guid getUserId);
    Task<PersonBase> RefreshAccess(string jwt);
}

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly ITulahackContext _tulahackContext;
    private readonly CdnConfiguration _cdnConfiguration;

    public AccountService(
        ITulahackContext tulahackContext,
        IOptions<CdnConfiguration> cdnConfiguration,
        IMapper mapper)
    {
        _tulahackContext = tulahackContext;
        _cdnConfiguration = cdnConfiguration.Value;
        _mapper = mapper;
    }

    public Task<PersonBaseDto?> CreateAccount(PersonBaseDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ContestantDto?> CreateContestant(ContestantDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ExpertDto?> CreateExpert(ExpertDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ModeratorDto?> CreateModerator(ModeratorDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<PersonBase> RefreshAccess(string jwt)
    {
        JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        PersonBase user = _tulahackContext.Accounts
            .AsTracking()
            .First(item => item.Id == Guid.Parse(token.Subject));

        user.Role = jwt.GetRole();
        await _tulahackContext.SaveChangesAsync();
        return user;
    }

    public async Task<PersonBase> CreateAccount(string jwt)
    {
        JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        var user = new PersonBase
        {
            Id = Guid.Parse(token.Subject),
            Firstname = token.Claims.First(item => item.Type == "given_name").Value,
            Lastname = token.Claims.First(item => item.Type == "family_name").Value,
            Email = token.Claims.First(item => item.Type == "email").Value,
            PhotoUrl = string.Concat(_cdnConfiguration.CdnUrl, "avatar/hacker.png"),
            Role = ContestRole.Visitor,
        };
        _ = _tulahackContext.Accounts.Add(user);
        await _tulahackContext.SaveChangesAsync();
        return user;
    }

    public async Task<PersonBase?> GetAccount(Guid userId)
    {
        PersonBase? account = await _tulahackContext
            .Accounts
            .FirstOrDefaultAsync(user => user.Id == userId);

        return account;
    }

    public async Task<ContestantDto?> GetContestantDetails(Guid userId)
    {
        Contestant? account = await _tulahackContext
            .Contestants
            .Include(item => item.Team)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (account is null)
        {
            return null;
        }

        List<Contestant> team = await _tulahackContext
            .Contestants
            .Where(item => item.Team.Id == account.Team.Id)
            .ToListAsync();

        ContestantDto result = _mapper.Map<ContestantDto>(account);
        result.Team.Contestants = _mapper.Map<List<ContestantDto>>(team);

        return result;
    }

    public async Task<ExpertDto?> GetExpertDetails(Guid userId)
    {
        Expert? account = await _tulahackContext
            .Experts
            .FirstOrDefaultAsync(user => user.Id == userId);

        return account is null ? null : _mapper.Map<ExpertDto>(account);
    }


    public async Task<ModeratorDto?> GetModeratorDetails(Guid userId)
    {
        Moderator? account = await _tulahackContext
            .Moderators
            .FirstOrDefaultAsync(user => user.Id == userId);

        return account is null ? null : _mapper.Map<ModeratorDto>(account);
    }

    public Task<PersonBaseDto?> UpdateAccount(PersonBaseDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ContestantDto?> UpdateContestant(ContestantDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ExpertDto?> UpdateExpert(ExpertDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ModeratorDto?> UpdateModerator(ModeratorDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<PersonBase?> DeleteAccount(Guid getUserId)
    {
        throw new NotImplementedException();
    }
}