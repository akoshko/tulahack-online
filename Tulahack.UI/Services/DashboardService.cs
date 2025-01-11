using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Tulahack.Dtos;

namespace Tulahack.UI.Services;

public interface IDashboardService
{
    public Task<DashboardDto> GetDashboardOverview();
}

[UnconditionalSuppressMessage("Trimming",
    "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
    Justification =
        "Passed custom SerializerOptions into method which has it's own JsonSerializerContext (TulahackJsonContext) specified")]
public class DashboardService(IHttpService httpService) : IDashboardService
{
    public async Task<DashboardDto> GetDashboardOverview()
    {
        DashboardDto result = await httpService.GetAndHandleAsync<DashboardDto>(
            new Uri("dashboard", UriKind.Relative),
            default);
        return result;
    }
}
