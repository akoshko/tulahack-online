using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Tulahack.Dtos;
using Tulahack.UI.Extensions;

namespace Tulahack.UI.Services;

public interface IDashboardService
{
    public Task<DashboardDto> GetDashboardOverview();
}

[UnconditionalSuppressMessage("Trimming",
    "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
    Justification =
        "Passed custom SerializerOptions into method which has it's own JsonSerializerContext (TulahackJsonContext) specified")]
public class DashboardService(
    HttpClient httpClient,
    JsonSerializerOptions serializerOptions,
    INotificationsService notificationsService)
    : IDashboardService
{
    public async Task<DashboardDto> GetDashboardOverview()
    {
        DashboardDto? result = await httpClient.GetAndHandleAsync<DashboardDto>(new Uri("dashboard", UriKind.Relative), serializerOptions);

        if (result is null)
        {
            throw new HttpRequestException("cannot get Dashboard data api/dashboard from server, result is null");
        }
        _ = notificationsService.ShowSuccess("Dashboard data api/dashboard from server");
        return result;
    }
}
