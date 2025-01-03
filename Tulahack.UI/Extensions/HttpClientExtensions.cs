using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tulahack.UI.Extensions;

[RequiresUnreferencedCode("T should be specified in TulahackJsonContext as [JsonSerializable(typeof(T))]")]
public static class HttpClientExtensions
{
    //
    // GET methods
    //
    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        JsonSerializerOptions serializerOptions)
        where T : class?
    {
        HttpResponseMessage response = await client.GetAsync(requestUri);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        HttpCompletionOption completionOption,
        JsonSerializerOptions serializerOptions)
        where T : class?
    {
        HttpResponseMessage response = await client.GetAsync(requestUri, completionOption);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async static Task<T?> GetAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        HttpCompletionOption completionOption,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await client.GetAsync(requestUri, completionOption, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }
    //
    // POST methods
    //
    public async static Task<T> PostAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        object content,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
    {
        using var body = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await client.PostAsync(requestUri, body, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async static Task<T?> PostAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        HttpContent? content,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await client.PostAsync(requestUri, content, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async static Task<HttpResponseMessage> PostJsonAsync<T>(
        this HttpClient client,
        Uri requestUri,
        T content,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
        where T : class?
    {
        using var json = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await client.PostAsync(requestUri, json, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return response;
    }

    public async static Task<HttpResponseMessage> PostJsonAsync(
        this HttpClient client,
        Uri requestUri,
        object content,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
    {
        using var json = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await client.PostAsync(requestUri, json, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return response;
    }
    //
    // PATCH methods
    //
    public async static Task<T?> PatchAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        HttpContent? content,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
        where T : class?
    {
        HttpResponseMessage response = await client.PatchAsync(requestUri, content, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        T? result = JsonSerializer.Deserialize<T>(json, serializerOptions);
        return result;
    }

    public async static Task<HttpResponseMessage> PatchAndHandleAsync<T>(
        this HttpClient client,
        Uri requestUri,
        T content,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
        where T : class?
    {
        using var json = JsonContent.Create(
            content,
            new MediaTypeHeaderValue("application/json"),
            serializerOptions);
        HttpResponseMessage response = await client.PatchAsync(requestUri, json, cancellationToken);
        _ = response.EnsureSuccessStatusCode();
        return response;
    }
}

