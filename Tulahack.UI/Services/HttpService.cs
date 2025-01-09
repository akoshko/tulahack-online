using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tulahack.UI.Utils;

namespace Tulahack.UI.Services;

[RequiresUnreferencedCode("T should be specified in TulahackJsonContext as [JsonSerializable(typeof(T))]")]
[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072:UnrecognizedReflectionPattern",
    Justification = "Implementation detail of Activator that linker intrinsically recognizes")]
public class HttpService(
    HttpClient httpClient,
    JsonSerializerOptions serializerOptions,
    INotificationsService notificationsService)
    : IHttpService
{
    private T ShowErrorMessageAndHandle<T>(Uri requestUri, Exception exception) where T : class
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
                    return Activator.CreateInstance<T>();
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
                    return Activator.CreateInstance<T>();
                default:
                    _ = notificationsService.ShowError($"{exception.Message}, URL: {requestUri.Query}");
                    return Activator.CreateInstance<T>();
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
        return Activator.CreateInstance<T>();
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

    private async static Task<object> SendJsRequest(Uri requestUri, string content, HttpMethod httpMethod) =>
        httpMethod switch
        {
            { Method: "GET" } => await JsExportedMethods.GetAsync(requestUri),
            { Method: "POST" } => await JsExportedMethods.PostAsync(requestUri, content),
            { Method: "PUT" } => await JsExportedMethods.PutAsync(requestUri, content),
            { Method: "DELETE" } => await JsExportedMethods.DeleteAsync(requestUri),
            { Method: "PATCH" } => await JsExportedMethods.PatchAsync(requestUri, content),
            _ => throw new ArgumentOutOfRangeException(nameof(httpMethod),
                $"Not expected HTTP method: {httpMethod.Method}"),
        };

    private async Task<string> SendRequestViaJsInterop(Uri requestUri, HttpMethod httpMethod, object? body = null)
    {
        var content = body is null ? string.Empty : JsonSerializer.Serialize(body);
        var response = await SendJsRequest(requestUri, content, httpMethod);
        return response switch
        {
            byte[] bytes => Encoding.UTF8.GetString(bytes),
            string text => text,
            _ => string.Empty
        };
    }

    private async Task<string> SendRequestViaHttpClient(Uri requestUri, HttpMethod httpMethod,
        CancellationToken cancellationToken, object? body = null)
    {
        using var request = new HttpRequestMessage(httpMethod, requestUri);

        if (body != null)
        {
            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");
        }

        HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    private async Task<string> SendRequestViaHttpClient(Uri requestUri, HttpMethod httpMethod,
        HttpCompletionOption completionOption, CancellationToken cancellationToken, object? body = null)
    {
        using var request = new HttpRequestMessage(httpMethod, requestUri);

        if (body != null)
        {
            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");
        }

        HttpResponseMessage response = await httpClient.SendAsync(request, completionOption, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<T> GetAndHandleAsync<T>(
        Uri requestUri,
        CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var json = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Get)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Get, cancellationToken);
            T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (Exception e)
        {
            return ShowErrorMessageAndHandle<T>(requestUri, e);
        }
    }

    public async Task<T> GetAndHandleAsync<T>(
        Uri requestUri,
        HttpCompletionOption completionOption,
        CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var json = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Get)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Get, completionOption, cancellationToken);
            T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (Exception e)
        {
            return ShowErrorMessageAndHandle<T>(requestUri, e);
        }
    }

    //
    // POST methods
    //
    public async Task<T> PostAndHandleAsync<T>(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var json = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Post)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Post, cancellationToken, body: content);
            T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (Exception e)
        {
            return ShowErrorMessageAndHandle<T>(requestUri, e);
        }
    }

    public async Task PostAndHandleAsync(
        Uri requestUri,
        object? content,
        CancellationToken cancellationToken)
    {
        try
        {
            _ = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Post)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Post, cancellationToken, body: content);
        }
        catch (Exception e)
        {
            ShowErrorMessageAndHandle(requestUri, e);
        }
    }

    //
    // PATCH methods
    //
    public async Task<T> PatchAndHandleAsync<T>(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var json = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Patch)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Patch, cancellationToken, body: content);
            T result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (Exception e)
        {
            return ShowErrorMessageAndHandle<T>(requestUri, e);
        }
    }

    public async Task PatchAndHandleAsync(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
    {
        try
        {
            _ = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Patch)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Patch, cancellationToken, body: content);
        }
        catch (Exception e)
        {
            ShowErrorMessageAndHandle(requestUri, e);
        }
    }

    //
    // PUT methods
    //
    public async Task<T> PutAndHandleAsync<T>(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var json = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Put)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Put, cancellationToken, body: content);
            T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (Exception e)
        {
            return ShowErrorMessageAndHandle<T>(requestUri, e);
        }
    }

    public async Task PutAndHandleAsync(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
    {
        try
        {
            _ = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Put)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Put, cancellationToken, body: content);
        }
        catch (Exception e)
        {
            ShowErrorMessageAndHandle(requestUri, e);
        }
    }

    //
    // DELETE methods
    //
    public async Task<T> DeleteAndHandleAsync<T>(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
        where T : class
    {
        try
        {
            var json = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Delete)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Delete, cancellationToken, body: content);
            T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return result;
        }
        catch (Exception e)
        {
            return ShowErrorMessageAndHandle<T>(requestUri, e);
        }
    }

    public async Task DeleteAndHandleAsync(
        Uri requestUri,
        object content,
        CancellationToken cancellationToken)
    {
        try
        {
            _ = OperatingSystem.IsBrowser()
                ? await SendRequestViaJsInterop(requestUri, HttpMethod.Delete)
                : await SendRequestViaHttpClient(requestUri, HttpMethod.Delete, cancellationToken, body: content);
        }
        catch (Exception e)
        {
            ShowErrorMessageAndHandle(requestUri, e);
        }
    }
}
