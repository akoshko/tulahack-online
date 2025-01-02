using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Tulahack.Dtos;
using Tulahack.UI.Extensions;

namespace Tulahack.UI.Services;

public interface IUserService
{
    public Task<T?> GetAccountAs<T>(string entityName) where T : class;
    public Task<ContestantDto?> GetAccount();
    Task SaveUserPreferences(UserPreferencesDto preferences);
}

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly INotificationsService _notificationsService;

    public UserService(
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
        Justification = "ContestantDto, ExpertDto, ModeratorDto, PersonBaseDto are specified in TulahackJsonContext")]
    public async Task<T?> GetAccountAs<T>(string entityName) where T : class
    {
        var url = entityName switch
        {
            nameof(ContestantDto) => "account/contestant",
            nameof(ExpertDto) => "account/expert",
            nameof(ModeratorDto) => "account/moderator",
            _ => "account"
        };

        T? result = await _httpClient.GetAndHandleAsync<T>(
            new Uri(url),
            _serializerOptions);
        return result;
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "ContestantDto is specified in TulahackJsonContext")]
    public async Task<ContestantDto?> GetAccount()
    {
        ContestantDto? result = await _httpClient.GetAndHandleAsync<ContestantDto>(
            new Uri("account"),
            _serializerOptions);
        return result;
    }

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access",
        Justification = "UserPreferencesDto is specified in TulahackJsonContext")]
    public async Task SaveUserPreferences(UserPreferencesDto preferences) =>
        await _httpClient.PostJsonAsync(
            new Uri("account/preferences"),
            preferences,
            serializerOptions: _serializerOptions,
            default);
}
