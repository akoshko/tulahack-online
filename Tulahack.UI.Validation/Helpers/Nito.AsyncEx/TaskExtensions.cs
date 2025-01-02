#nullable disable


using System;
using System.Threading;
using System.Threading.Tasks;

// Original code from StephenCleary: https://github.com/StephenCleary/AsyncEx

namespace Tulahack.UI.Validation.Helpers.Nito.AsyncEx;

/// <summary>
/// Provides synchronous extension methods for tasks.
/// </summary>
internal static class TaskExtensions
{
    /// <summary>
    /// Waits for the task to complete, unwrapping any exceptions.
    /// </summary>
    /// <param name="task">The task. May not be <c>null</c>.</param>
    public static void WaitAndUnwrapException(this Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Waits for the task to complete, unwrapping any exceptions.
    /// </summary>
    /// <param name="task">The task. May not be <c>null</c>.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> was cancelled before the <paramref name="task"/> completed, or the <paramref name="task"/> raised an <see cref="OperationCanceledException"/>.</exception>
    public static void WaitAndUnwrapException(this Task task, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(task);

        try
        {
            task.Wait(cancellationToken);
        }
        catch (AggregateException ex)
        {
            throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
        }
    }

    /// <summary>
    /// Waits for the task to complete, unwrapping any exceptions.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the task.</typeparam>
    /// <param name="task">The task. May not be <c>null</c>.</param>
    /// <returns>The result of the task.</returns>
    public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
    {
        ArgumentNullException.ThrowIfNull(task);

        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Asynchronously waits for the task to complete, or for the cancellation token to be canceled.
    /// </summary>
    /// <param name="this">The task to wait for. May not be <c>null</c>.</param>
    /// <param name="cancellationToken">The cancellation token that cancels the wait.</param>
    public static Task WaitAsync(this Task @this, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@this);

        if (!cancellationToken.CanBeCanceled)
        {
            return @this;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        return DoWaitAsync(@this, cancellationToken);
    }

    private async static Task DoWaitAsync(Task task, CancellationToken cancellationToken)
    {
        using var cancelTaskSource = new CancellationTokenTaskSource<object>(cancellationToken);

        await (await Task.WhenAny(task, cancelTaskSource.Task).ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously waits for the task to complete, or for the cancellation token to be canceled.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="this">The task to wait for. May not be <c>null</c>.</param>
    /// <param name="cancellationToken">The cancellation token that cancels the wait.</param>
    public static Task<TResult> WaitAsync<TResult>(this Task<TResult> @this, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@this);

        if (!cancellationToken.CanBeCanceled)
        {
            return @this;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<TResult>(cancellationToken);
        }

        return DoWaitAsync(@this, cancellationToken);
    }

    private async static Task<TResult> DoWaitAsync<TResult>(Task<TResult> task, CancellationToken cancellationToken)
    {
        using var cancelTaskSource = new CancellationTokenTaskSource<TResult>(cancellationToken);

        return await (await Task.WhenAny(task, cancelTaskSource.Task).ConfigureAwait(false)).ConfigureAwait(false);
    }
}
