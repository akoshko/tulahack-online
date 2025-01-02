using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tulahack.UI.Services;

namespace Tulahack.UI.Extensions;

[RequiresUnreferencedCode("T should be specified in TulahackJsonContext as [JsonSerializable(typeof(T))]")]
public static class HttpClientExtensions
{
    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri,
        JsonSerializerOptions serializerOptions,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
        where T : class
    {
        try
        {
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }

        return null;
    }

    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, 
        HttpCompletionOption completionOption,
        JsonSerializerOptions serializerOptions,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
        where T : class
    {
        try
        {
            var response = await client.GetAsync(requestUri, completionOption);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }

        return null;
    }

    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri,
        CancellationToken cancellationToken,
        JsonSerializerOptions serializerOptions,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
        where T : class
    {
        try
        {
            var response = await client.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }

        return null;
    }

    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, 
        HttpCompletionOption completionOption,
        CancellationToken cancellationToken,
        JsonSerializerOptions serializerOptions,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
        where T : class
    {
        try
        {
            var response = await client.GetAsync(requestUri, completionOption, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }

        return null;
    }
    
    public async static Task<T?> PostAndHandleAsync<T>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri,
        HttpContent? content,
        CancellationToken cancellationToken,
        JsonSerializerOptions serializerOptions,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
        where T : class
    {
        try
        {
            var response = await client.PostAsync(requestUri, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }

        return null;
    }
    
    public async static Task PostAsJsonAsync<T>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri,
        T content,
        CancellationToken cancellationToken,
        JsonSerializerOptions serializerOptions,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
        where T : class
    {
        try
        {
            var json = JsonContent.Create(
                content, 
                mediaType: new MediaTypeHeaderValue("application/json"),
                options: serializerOptions);
            var response = await client.PostAsync(requestUri, json, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }
    }
    
    public async static Task PostAndHandleAsync(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri,
        HttpContent? content,
        CancellationToken cancellationToken,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
    {
        try
        {
            var response = await client.PostAsync(requestUri, content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }
    }
    
    public async static Task<T?> PatchAndHandleAsync<T>(
        this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri,
        HttpContent? content,
        CancellationToken cancellationToken,
        JsonSerializerOptions serializerOptions,
        INotificationsService? notificationsService = default,
        string? networkErrorMessage = default,
        string? criticalErrorMessage = default)
        where T : class
    {
        try
        {
            var response = await client.PatchAsync(requestUri, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (HttpRequestException e)
        {
            notificationsService?.ShowError(networkErrorMessage ?? e.Message);
        }
        catch (Exception e)
        {
            notificationsService?.ShowError(criticalErrorMessage ?? e.Message);
        }

        return null;
    }
}