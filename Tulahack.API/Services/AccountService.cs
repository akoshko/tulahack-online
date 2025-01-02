using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Tulahack.API.Context;
using Tulahack.API.Extensions;
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

    public AccountService(
        ITulahackContext tulahackContext,
        IMapper mapper)
    {
        _tulahackContext = tulahackContext;
        _mapper = mapper;
    }

    public async Task<PersonBaseDto?> CreateAccount(PersonBaseDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ContestantDto?> CreateContestant(ContestantDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ExpertDto?> CreateExpert(ExpertDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ModeratorDto?> CreateModerator(ModeratorDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<PersonBase> RefreshAccess(string jwt)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        var user = _tulahackContext.Accounts
            .AsTracking()
            .First(item => item.Id == Guid.Parse(token.Subject));

        user.Role = jwt.GetRole();
        await _tulahackContext.SaveChangesAsync();
        return user;
    }

    public async Task<PersonBase> CreateAccount(string jwt)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        var user = new PersonBase
        {
            Id = Guid.Parse(token.Subject),
            Firstname = token.Claims.First(item => item.Type == "given_name").Value,
            Lastname = token.Claims.First(item => item.Type == "family_name").Value,
            Email = token.Claims.First(item => item.Type == "email").Value,
            PhotoUrl = "https://cdn.<you-name-it>/avatar/hacker.png",
            Role = ContestRole.Visitor,
        };
        _tulahackContext.Accounts.Add(user);
        await _tulahackContext.SaveChangesAsync();
        return user;
    }

    public async Task<PersonBase?> GetAccount(Guid userId)
    {
        var account = await _tulahackContext
            .Accounts
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (account is null)
            return null;

        return account;
    }

    public async Task<ContestantDto?> GetContestantDetails(Guid userId)
    {
        var account = await _tulahackContext
            .Contestants
            .Include(item => item.Team)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (account is null)
            return null;

        var team = await _tulahackContext
            .Contestants
            .Where(item => item.Team.Id == account.Team.Id)
            .ToListAsync();

        var result = _mapper.Map<ContestantDto>(account);
        result.Team.Contestants = _mapper.Map<List<ContestantDto>>(team);

        return result;
    }

    public async Task<ExpertDto?> GetExpertDetails(Guid userId)
    {
        var account = await _tulahackContext
            .Experts
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (account is null)
            return null;

        return _mapper.Map<ExpertDto>(account);
    }


    public async Task<ModeratorDto?> GetModeratorDetails(Guid userId)
    {
        var account = await _tulahackContext
            .Moderators
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (account is null)
            return null;

        return _mapper.Map<ModeratorDto>(account);
    }

    public async Task<PersonBaseDto?> UpdateAccount(PersonBaseDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ContestantDto?> UpdateContestant(ContestantDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ExpertDto?> UpdateExpert(ExpertDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ModeratorDto?> UpdateModerator(ModeratorDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<PersonBase?> DeleteAccount(Guid getUserId)
    {
        throw new NotImplementedException();
    }
}