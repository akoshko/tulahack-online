using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tulahack.UI.Services;

[RequiresUnreferencedCode("T should be specified in TulahackJsonContext as [JsonSerializable(typeof(T))]")]
public class HttpService(
    HttpClient httpClient,
    JsonSerializerOptions serializerOptions,
    INotificationsService notificationsService)
    : IHttpService
{
    private T ShowErrorMessageAndHandle<T>(Uri requestUri, Exception exception) where T : class?
    {
        if (exception is HttpRequestException httpRequestException)
        {
            switch (httpRequestException.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    _ = notificationsService.ShowWarning($"{exception.Message}, URL: {requestUri.Query}");
                    return Activator.CreateInstance<T>()!;
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
                    return Activator.CreateInstance<T>()!;
                default:
                    _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
                    return Activator.CreateInstance<T>()!;
            }
        }

        if (exception is ArgumentNullException argumentNullException)
        {
            _ = notificationsService.ShowError($"{argumentNullException.Message}, URL: {requestUri.Query}");
            return null;
        }

        if (exception is JsonException jsonException)
        {
            _ = notificationsService.ShowError($"{jsonException.Message}, URL: {requestUri.Query}");
            return null;
        }

        if (exception is NotSupportedException notSupportedException)
        {
            _ = notificationsService.ShowError($"{notSupportedException.Message}, URL: {requestUri.Query}");
            return null;
        }

        _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
        return null;
    }

    private void ShowErrorMessageAndHandle(Uri requestUri, Exception exception)
    {
        if (exception is HttpRequestException httpRequestException)
        {
            switch (httpRequestException.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    _ = notificationsService.ShowWarning($"{exception.Message}, URL: {requestUri.Query}");
                    return;
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
                    return;
                default:
                    _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
                    return;
            }
        }

        if (exception is ArgumentNullException argumentNullException)
        {
            _ = notificationsService.ShowError($"{argumentNullException.Message}, URL: {requestUri.Query}");
        }

        if (exception is JsonException jsonException)
        {
            _ = notificationsService.ShowError($"{jsonException.Message}, URL: {requestUri.Query}");
        }

        if (exception is NotSupportedException notSupportedException)
        {
            _ = notificationsService.ShowError($"{notSupportedException.Message}, URL: {requestUri.Query}");
        }

        _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
    }

    public async Task<T?> GetAndHandleAsync<T>(Uri requestUri) where T : class?
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);
            _ = response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (Exception e)
        {
            return ShowErrorMessageAndHandle<T>(requestUri, e);
        }
    }

    public async Task<T?> GetAndHandleAsync<T>(
        Uri requestUri,
        HttpCompletionOption completionOption)
        where T : class?
    {
        HttpResponseMessage response = await httpClient.GetAsync(requestUri, completionOption);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async Task<T?> GetAndHandleAsync<T>(
        Uri requestUri,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async Task<T?> GetAndHandleAsync<T>(
        Uri requestUri,
        HttpCompletionOption completionOption,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await httpClient.GetAsync(requestUri, completionOption, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    //
    // POST methods
    //
    public async Task<T> PostAndHandleAsync<T>(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
    {
        using var body = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, body, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async Task<T?> PostAndHandleAsync<T>(
        Uri requestUri,
        HttpContent? content,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, content, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async Task<bool> PostAndHandleAsync(
        Uri requestUri,
        HttpContent? content,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, content, cancellationToken);
        HttpResponseMessage result = response.EnsureSuccessStatusCode();
        return result.IsSuccessStatusCode;
    }

    public async Task<HttpResponseMessage> PostJsonAsync<T>(
        Uri requestUri,
        T content,
        CancellationToken cancellationToken)
        where T : class?
    {
        using var json = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, json, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return response;
    }

    public async Task<HttpResponseMessage> PostJsonAsync(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
    {
        using var json = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, json, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return response;
    }

    //
    // PATCH methods
    //
    public async Task<T?> PatchAndHandleAsync<T>(
        Uri requestUri,
        HttpContent? content,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await httpClient.PatchAsync(requestUri, content, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async Task<HttpResponseMessage> PatchAndHandleAsync<T>(
        Uri requestUri,
        T content,
        CancellationToken cancellationToken)
        where T : class?
    {
        using var json = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await httpClient.PatchAsync(requestUri, json, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return response;
    }
}
