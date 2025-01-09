using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tulahack.UI.Services;

public interface IHttpService
{
    Task<T> GetAndHandleAsync<T>(Uri requestUri, CancellationToken cancellationToken) where T : class;
    Task<T> PostAndHandleAsync<T>(Uri requestUri, object content, CancellationToken cancellationToken) where T : class;
    Task PostAndHandleAsync(Uri requestUri, object? content, CancellationToken cancellationToken);
    Task<T> PatchAndHandleAsync<T>(Uri requestUri, object content, CancellationToken cancellationToken) where T : class;
    Task PatchAndHandleAsync(Uri requestUri, object content, CancellationToken cancellationToken);
    Task<T> PutAndHandleAsync<T>(Uri requestUri, object content, CancellationToken cancellationToken) where T : class;
    Task PutAndHandleAsync(Uri requestUri, object content, CancellationToken cancellationToken);
    Task<T> DeleteAndHandleAsync<T>(Uri requestUri, object content, CancellationToken cancellationToken) where T : class;
    Task DeleteAndHandleAsync(Uri requestUri, object content, CancellationToken cancellationToken);
}
