using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
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
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly INotificationsService _notificationsService;
    private readonly ILogger<TeamService> _logger;

    public TeamService(
        HttpClient httpClient,
        JsonSerializerOptions serializerOptions,
        INotificationsService notificationsService,
        ILogger<TeamService> logger)
    {
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
        _notificationsService = notificationsService;

        _logger = logger;
    }

    public async Task<TeamDto?> GetTeam()
    {
        TeamDto? result = await _httpClient.GetAndHandleAsync<TeamDto>(
            new Uri("teams"),
            _serializerOptions);
        return result;
    }


    public async Task<TeamDto?> GetTeamById(Guid teamId)
    {
        TeamDto? result = await _httpClient.GetAndHandleAsync<TeamDto>(
            new Uri($"teams/{teamId}"),
            _serializerOptions);
        return result;
    }

    public async Task<List<StorageFileDto>> GetStorageFiles()
    {
        List<StorageFileDto>? result = await _httpClient.GetAndHandleAsync<List<StorageFileDto>>(
            new Uri("storage/files"),
            _serializerOptions);
        return result;
    }

    public async Task<List<StorageFileDto>> GetStorageFiles(Guid teamId)
    {
        List<StorageFileDto>? result = await _httpClient.GetAndHandleAsync<List<StorageFileDto>>(
            new Uri($"storage/{teamId}/files"),
            _serializerOptions);
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
        _ = await _httpClient
            .PostAndHandleAsync<object>(
                requestUri: new Uri("storage?purposeType=SolutionTeaser"),
                content: content,
                serializerOptions: _serializerOptions,
                cancellationToken: default);
        _ = _notificationsService.ShowSuccess($"Тизер успешно загружен!");
        _logger.LogInformation("Successfully uploaded teaser file");
    }

    public async Task JoinTeam(ContestApplicationDto application)
    {
        _ = await _httpClient
            .PostJsonAsync(
                new Uri("teams/join"),
                application,
                cancellationToken: default,
                serializerOptions: _serializerOptions);
        _ = _notificationsService.ShowSuccess($"Заявка успешно отправлена! Дождитесь ответа организаторов.");
    }

    public async Task CreateTeam(ContestApplicationDto application)
    {
        _ = await _httpClient
            .PostJsonAsync(
                new Uri("teams/create"),
                application,
                cancellationToken: default,
                serializerOptions: _serializerOptions);
        _ = _notificationsService.ShowSuccess($"Заявка успешно отправлена! Дождитесь ответа организаторов.");
    }
}
