using System;
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
        _ = await _httpClient.PostJsonAsync(
            new Uri("application", UriKind.Relative),
            dto,
            _serializerOptions,
            cancellationToken);

        _ = _notificationsService.ShowSuccess();
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task ApproveApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default)
    {
        using var content = new StringContent(dto.StatusJustification);
        _ = await _httpClient.PatchAndHandleAsync<ContestApplicationDto>(
            new Uri($"application/{dto.Id}/approve", UriKind.Relative),
            content,
            _serializerOptions,
            cancellationToken);

        _ = _notificationsService.ShowSuccess();
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task DeclineApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default)
    {
        using var content = new StringContent(dto.StatusJustification);
        _ = await _httpClient.PatchAndHandleAsync<ContestApplicationDto>(
            new Uri($"application/{dto.Id}/decline", UriKind.Relative),
            content,
            _serializerOptions,
            cancellationToken);

        _ = _notificationsService.ShowSuccess();
    }
}
