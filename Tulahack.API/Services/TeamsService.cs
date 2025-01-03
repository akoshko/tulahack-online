using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tulahack.API.Context;
using Tulahack.Dtos;
using Tulahack.Model;

namespace Tulahack.API.Services;

public interface ITeamsService
{
    Task<TeamDto?> GetTeam(Guid teamId);
    Task<TeamDto?> GetTeamByUserId(Guid teamId);
    Task<TeamDto> CreateTeam(Guid userId, ContestApplicationDto dto);
    Task<TeamDto> JoinTeam(Guid userId, ContestApplicationDto dto);
}

public class TeamsService : ITeamsService
{
    private readonly ITulahackContext _context;
    private readonly IMapper _mapper;

    public TeamsService(ITulahackContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeamDto?> GetTeamByUserId(Guid userId)
    {
        Contestant? contestant = await _context.Contestants
            .Include(item => item.Team)
                .ThenInclude(team => team.Contestants)
            .Include(item => item.Team)
                .ThenInclude(team => team.ContestCases)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == userId);

        if (contestant?.Team is null)
        {
            return null;
        }

        return _mapper.Map<TeamDto>(contestant.Team);
    }

    public async Task<TeamDto?> GetTeam(Guid teamId)
    {
        Team? team = await _context.Teams
            .Include(team => team.Contestants)
            .Include(team => team.ContestCases)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == teamId);

        if (team is null)
        {
            return null;
        }

        return _mapper.Map<TeamDto>(team);
    }

    public async Task<TeamDto> CreateTeam(Guid userId, ContestApplicationDto dto)
    {
        Contestant leader = await _context.Contestants
            .AsTracking()
            .FirstAsync(item => item.Id == userId);
        var team = new Team
        {
            Name = dto.TeamName,
            Contestants = new[] { leader },
            Section = dto.Section,
            AdditionalInfo = dto.AboutTeam,
            FormOfParticipation = Enum.Parse<FormOfParticipation>(dto.FormOfParticipation.ToString())
        };
        leader.Team = team;
        _ = _context.Contestants.Update(leader);
        _ = _context.Teams.Add(team);

        await _context.SaveChangesAsync();
        return _mapper.Map<TeamDto>(team);
    }

    public async Task<TeamDto> JoinTeam(Guid userId, ContestApplicationDto dto)
    {
        Contestant contestant = _context.Contestants
            .AsTracking()
            .First(item => item.Id == userId);
        if (dto.JoinExistingTeam is false || dto.ExistingTeamId is null)
        {
            throw new ValidationException("Unable to find team for provided application, aborting");
        }

        Team team = await _context.Teams
            .AsTracking()
            .FirstAsync(item => item.Id == dto.ExistingTeamId);

        contestant.Team = team;
        await _context.SaveChangesAsync();
        return _mapper.Map<TeamDto>(team);
    }
}