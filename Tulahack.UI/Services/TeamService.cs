using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
        _logger = logger;
        _notificationsService = notificationsService;
    }

    public async Task<TeamDto?> GetTeam()
    {
        var result = await _httpClient.GetAndHandleAsync<TeamDto>(
            "teams", 
            _serializerOptions, 
            _notificationsService);
        return result;
    }
    

    public async Task<TeamDto?> GetTeamById(Guid teamId)
    {
        var result = await _httpClient.GetAndHandleAsync<TeamDto>(
            $"teams/{teamId}", 
            _serializerOptions, 
            _notificationsService);
        return result;
    }

    public async Task<List<StorageFileDto>> GetStorageFiles()
    {
        var result = await _httpClient.GetAndHandleAsync<List<StorageFileDto>>(
            "storage/files", 
            _serializerOptions,
            _notificationsService);
        return result;
    }
    
    public async Task<List<StorageFileDto>> GetStorageFiles(Guid teamId)
    {
        var result = await _httpClient.GetAndHandleAsync<List<StorageFileDto>>(
            $"storage/{teamId}/files", 
            _serializerOptions,
            _notificationsService);
        return result;
    }

    public async Task UploadTeaser(IStorageFile file)
    {
        var stream = await file.OpenReadAsync();
        await _httpClient
            .PostAndHandleAsync(
                "storage?purposeType=SolutionTeaser",
                new MultipartFormDataContent
                {
                    {
                        new StreamContent(stream),
                        "files",
                        file.Name
                    }
                },
                cancellationToken: default);
        _notificationsService.ShowSuccess($"Тизер успешно загружен!");
    }

    public async Task JoinTeam(ContestApplicationDto application)
    {
        await _httpClient
            .PostAsJsonAsync(
                "teams/join",
                application,
                cancellationToken: default,
                serializerOptions: _serializerOptions);
        _notificationsService.ShowSuccess($"Заявка успешно отправлена! Дождитесь ответа организаторов.");
    }
    
    public async Task CreateTeam(ContestApplicationDto application)
    {
        await _httpClient
            .PostAsJsonAsync(
                "teams/create",
                application,
                cancellationToken: default,
                serializerOptions: _serializerOptions);
        _notificationsService.ShowSuccess($"Заявка успешно отправлена! Дождитесь ответа организаторов.");
    }
}