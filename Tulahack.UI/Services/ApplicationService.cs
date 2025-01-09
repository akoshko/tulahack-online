using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Tulahack.Dtos;

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
    private readonly IHttpService _httpClient;

    public ApplicationService(IHttpService httpService)
    {
        _httpClient = httpService;
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task SubmitApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default) => await _httpClient.PostAndHandleAsync(
        new Uri("application", UriKind.Relative),
        dto,
        cancellationToken);

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task ApproveApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default) =>
        _ = await _httpClient.PatchAndHandleAsync<ContestApplicationDto>(
            new Uri($"application/{dto.Id}/approve", UriKind.Relative),
            dto.StatusJustification,
            cancellationToken);

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestApplicationDto is specified in TulahackJsonContext")]
    public async Task DeclineApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default) =>
        _ = await _httpClient.PatchAndHandleAsync<ContestApplicationDto>(
            new Uri($"application/{dto.Id}/decline", UriKind.Relative),
            dto,
            cancellationToken);
}
