using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tulahack.Dtos;
using Tulahack.UI.Extensions;

namespace Tulahack.UI.Services;

public interface IApplicationService
{
    Task SubmitApplicationAsync(
        ContestApplicationDto dto, 
        CancellationToken cancellationToken = default);
    
    Task ApproveApplicationAsync(
        ContestApplicationDto dto, 
        CancellationToken cancellationToken = default);
    
    Task DeclineApplicationAsync(
        ContestApplicationDto dto, 
        CancellationToken cancellationToken = default);
}

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly INotificationsService _notificationsService;

    public ApplicationService(
        HttpClient httpClient,
        JsonSerializerOptions serializerOptions,
        INotificationsService notificationsService)
    {
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
        _notificationsService = notificationsService;
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task SubmitApplicationAsync(ContestApplicationDto dto, CancellationToken cancellationToken = default)
    {
        await _httpClient.PostAsJsonAsync(
            "application", 
            dto,
            cancellationToken,
            _serializerOptions,
            _notificationsService);
        
        _notificationsService.ShowSuccess();
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task ApproveApplicationAsync(
        ContestApplicationDto dto, 
        CancellationToken cancellationToken = default)
    {
        await _httpClient.PatchAndHandleAsync<ContestApplicationDto>(
            $"application/{dto.Id}/approve", 
            new StringContent(dto.StatusJustification),
            cancellationToken,
            _serializerOptions,
            _notificationsService);
        
        _notificationsService.ShowSuccess();
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task DeclineApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default)
    {
        await _httpClient.PatchAndHandleAsync<ContestApplicationDto>(
            $"application/{dto.Id}/decline", 
            new StringContent(dto.StatusJustification),
            cancellationToken,
            _serializerOptions,
            _notificationsService);
        
        _notificationsService.ShowSuccess();
    }
}