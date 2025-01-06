using Microsoft.EntityFrameworkCore;
using Tulahack.API.Context;
using Tulahack.API.Utils;
using Tulahack.Dtos;

namespace Tulahack.API.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetOverview();
}

public class DashboardService : IDashboardService
{
    private readonly ITulahackContext _context;
    private readonly CdnConfiguration _cdnConfiguration;

    public DashboardService(
        ITulahackContext context,
        CdnConfiguration cdnConfiguration)
    {
        _context = context;
        _cdnConfiguration = cdnConfiguration;
    }

    public async Task<DashboardDto> GetOverview()
    {
        var result = new DashboardDto
        {
            ParticipatorsCount = await _context.Accounts.CountAsync(),
            CompaniesCount = await _context.Companies.CountAsync(),
            ExpertsCount = await _context.Experts.CountAsync(),
            TeamsCount = await _context.Teams.CountAsync(),
            CasesCount = await _context.ContestCases.CountAsync(),
            TopicThumbnailUrl = string.Concat(_cdnConfiguration.CdnUrl, "events/welcome.png"),
            UpcomingEventId = Guid.NewGuid(),
            Timeline = new ContestTimelineDto
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
            }
        };

        return result;
    }
}