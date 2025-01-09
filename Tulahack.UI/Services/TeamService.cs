using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Avalonia.Platform.Storage;
using Tulahack.Dtos;
using Tulahack.UI.Extensions;

namespace Tulahack.UI.Services;

public interface ITeamService
{
    Task<TeamDto?> GetTeam();
    Task<TeamDto?> GetTeamById(Guid teamId);
    Task UploadTeaser(IStorageFile file);
    Task<List<StorageFileDto>> GetStorageFiles();
    Task<List<StorageFileDto>> GetStorageFiles(Guid teamId);
    Task JoinTeam(ContestApplicationDto application);
    Task CreateTeam(ContestApplicationDto application);
}

[UnconditionalSuppressMessage("Trimming",
    "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
    Justification = "TeamDto is specified in TulahackJsonContext")]
public class TeamService : ITeamService
{
    private readonly IHttpService _httpService;
    private readonly HttpClient _httpClient;
    private readonly INotificationsService _notificationsService;
    private readonly ILogger<TeamService> _logger;

    public TeamService(
        IHttpService httpService,
        ILogger<TeamService> logger,
        HttpClient httpClient,
        INotificationsService notificationsService)
    {
        _httpService = httpService;
        _logger = logger;
        _httpClient = httpClient;
        _notificationsService = notificationsService;
    }

    public async Task<TeamDto?> GetTeam()
    {
        TeamDto result = await _httpService.GetAndHandleAsync<TeamDto>(new Uri("teams", UriKind.Relative), default);
        return result;
    }

    public async Task<TeamDto?> GetTeamById(Guid teamId)
    {
        TeamDto result = await _httpService.GetAndHandleAsync<TeamDto>(
            new Uri($"teams/{teamId}", UriKind.Relative), default);
        return result;
    }

    public async Task<List<StorageFileDto>> GetStorageFiles()
    {
        List<StorageFileDto> result = await _httpService.GetAndHandleAsync<List<StorageFileDto>>(
            new Uri("storage/files", UriKind.Relative), default);
        return result;
    }

    public async Task<List<StorageFileDto>> GetStorageFiles(Guid teamId)
    {
        List<StorageFileDto> result = await _httpService.GetAndHandleAsync<List<StorageFileDto>>(
            new Uri($"storage/{teamId}/files", UriKind.Relative), default);
        return result;
    }

    public async Task UploadTeaser(IStorageFile file)
    {
        _logger.LogInformation("Sending teaser file");
        System.IO.Stream stream = await file.OpenReadAsync();
        using var streamContent = new StreamContent(stream);
        // ReSharper disable once UsingStatementResourceInitialization
        using var content = new MultipartFormDataContent
        {
            { streamContent, "files", file.Name }
        };
        var result = await _httpClient
            .PostAndHandleAsync(
                requestUri: new Uri("storage?purposeType=SolutionTeaser", UriKind.Relative),
                content: content,
                cancellationToken: default);

        if (result)
        {
            _ = _notificationsService.ShowSuccess($"Тизер успешно загружен!");
            _logger.LogInformation("Successfully uploaded teaser file");
            return;
        }

        _ = _notificationsService.ShowSuccess($"Не удалось загрузить тизер, повторите попытку");
        _logger.LogInformation("File upload failed");
    }

    public async Task JoinTeam(ContestApplicationDto application) => await _httpService
        .PostAndHandleAsync(
            new Uri("teams/join", UriKind.Relative),
            application,
            cancellationToken: default);

    public async Task CreateTeam(ContestApplicationDto application) => await _httpService
        .PostAndHandleAsync(
            new Uri("teams/create", UriKind.Relative),
            application,
            cancellationToken: default);
}
