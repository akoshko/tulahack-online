using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tulahack.UI.Services;

public interface IHttpService
{
    Task<T?> GetAndHandleAsync<T>(Uri requestUri) where T : class?;
    Task<T?> GetAndHandleAsync<T>(Uri requestUri, HttpCompletionOption completionOption) where T : class?;
    Task<T?> GetAndHandleAsync<T>(Uri requestUri, CancellationToken cancellationToken) where T : class?;
    Task<T?> GetAndHandleAsync<T>(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) where T : class?;
    Task<T> PostAndHandleAsync<T>(Uri requestUri, object content, CancellationToken cancellationToken);
    Task<T?> PostAndHandleAsync<T>(Uri requestUri, HttpContent? content, CancellationToken cancellationToken) where T : class?;
    Task<bool> PostAndHandleAsync(Uri requestUri, HttpContent? content, CancellationToken cancellationToken);
    Task<HttpResponseMessage> PostJsonAsync<T>(Uri requestUri, T content, CancellationToken cancellationToken) where T : class?;
    Task<HttpResponseMessage> PostJsonAsync(Uri requestUri, object content, CancellationToken cancellationToken);
    Task<T?> PatchAndHandleAsync<T>(Uri requestUri, HttpContent? content, CancellationToken cancellationToken) where T : class?;
    Task<HttpResponseMessage> PatchAndHandleAsync<T>(Uri requestUri, T content, CancellationToken cancellationToken) where T : class?;
}
