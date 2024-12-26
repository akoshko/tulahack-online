using Microsoft.EntityFrameworkCore;
using Tulahack.API.Context;
using Tulahack.Dtos;

namespace Tulahack.API.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetOverview();
}

public class DashboardService : IDashboardService
{
    private readonly ITulahackContext _context;

    public DashboardService(ITulahackContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetOverview()
    {
        var result = new DashboardDto();

        result.ParticipatorsCount = await _context.Accounts.CountAsync();
        result.CompaniesCount = await _context.Companies.CountAsync();
        result.ExpertsCount = await _context.Experts.CountAsync();
        result.TeamsCount = await _context.Teams.CountAsync();
        result.CasesCount = await _context.ContestCases.CountAsync();
        result.TopicThumbnailUrl = "https://cdn.<you-name-it>/events/welcome.png";
        result.UpcomingEventId = Guid.NewGuid();
        result.Timeline = new ContestTimelineDto
        {
            CodingBeginning = DateTime.Now.AddHours(2),
            CodingDeadline = DateTime.Now.AddHours(42),
            HackathonStart = DateTime.Now,
            HackathonEnd = DateTime.Now.AddHours(48),
            Items = new[]
            {
                new TimelineItemDto
                {
                    Start = DateTime.Now.AddHours(10),
                    Deadline = DateTime.Now.AddHours(11),
                    End = DateTime.Now.AddHours(12),
                    Label = "Checkpoint N1",
                    ItemType = TimelineItemTypeDto.Checkpoint,
                    Message = "Checkpoint is coming!",
                    Extra = "More",
                    Url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
                }
            }
        };

        return result;
    }
}