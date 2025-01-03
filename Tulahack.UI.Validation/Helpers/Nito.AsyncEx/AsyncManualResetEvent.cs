using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

// Original idea by Stephen Toub: http://blogs.msdn.com/b/pfxteam/archive/2012/02/11/10266920.aspx
// Original code from StephenCleary: https://github.com/StephenCleary/AsyncEx

namespace Tulahack.UI.Validation.Helpers.Nito.AsyncEx;

/// <summary>
/// An async-compatible manual-reset event.
/// </summary>
[DebuggerDisplay("Id = {Id}, IsSet = {GetStateForDebugger}")]
[DebuggerTypeProxy(typeof(DebugView))]
internal sealed class AsyncManualResetEvent
{
    /// <summary>
    /// The object used for synchronization.
    /// </summary>
    private readonly object _mutex;

    /// <summary>
    /// The current state of the event.
    /// </summary>
    private TaskCompletionSource<object?> _tcs;

    /// <summary>
    /// The semi-unique identifier for this instance. This is 0 if the id has not yet been created.
    /// </summary>
    private int _id;

    [DebuggerNonUserCode]
    // ReSharper disable once InconsistentlySynchronizedField
    private bool GetStateForDebugger => _tcs.Task.IsCompleted;

    /// <summary>
    /// Creates an async-compatible manual-reset event.
    /// </summary>
    /// <param name="set">Whether the manual-reset event is initially set or unset.</param>
    public AsyncManualResetEvent(bool set)
    {
        _mutex = new object();
        _tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<object?>();

        if (set)
        {
            _ = _tcs.TrySetResult(null);
        }
    }

    /// <summary>
    /// Creates an async-compatible manual-reset event that is initially unset.
    /// </summary>
    public AsyncManualResetEvent()
        : this(false) { }

    /// <summary>
    /// Gets a semi-unique identifier for this asynchronous manual-reset event.
    /// </summary>
    public int Id => IdManager<AsyncManualResetEvent>.GetId(ref _id);

    /// <summary>
    /// Whether this event is currently set. This member is seldom used; code using this member has a high possibility of race conditions.
    /// </summary>
    public bool IsSet
    {
        get
        {
            lock (_mutex)
            {
                return _tcs.Task.IsCompleted;
            }
        }
    }

    /// <summary>
    /// Asynchronously waits for this event to be set.
    /// </summary>
    public Task WaitAsync()
    {
        lock (_mutex)
        {
            return _tcs.Task;
        }
    }

    /// <summary>
    /// Asynchronously waits for this event to be set or for the wait to be canceled.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the wait. If this token is already canceled, this method will first check whether the event is set.</param>
    public Task WaitAsync(CancellationToken cancellationToken)
    {
        Task waitTask = WaitAsync();

        if (waitTask.IsCompleted)
        {
            return waitTask;
        }

        return waitTask.WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Synchronously waits for this event to be set. This method may block the calling thread.
    /// </summary>
    public void Wait() =>
        WaitAsync().WaitAndUnwrapException();

    /// <summary>
    /// Synchronously waits for this event to be set. This method may block the calling thread.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the wait. If this token is already canceled, this method will first check whether the event is set.</param>
    public void Wait(CancellationToken cancellationToken)
    {
        Task ret = WaitAsync(CancellationToken.None);

        if (ret.IsCompleted)
        {
            return;
        }

        ret.WaitAndUnwrapException(cancellationToken);
    }

#pragma warning disable CA1200 // Avoid using cref tags with a prefix
    /// <summary>
    /// Sets the event, atomically completing every task returned by <see cref="O:Nito.AsyncEx.AsyncManualResetEvent.WaitAsync"/>. If the event is already set, this method does nothing.
    /// </summary>
#pragma warning restore CA1200 // Avoid using cref tags with a prefix
    public void Set()
    {
        lock (_mutex)
        {
            _ = _tcs.TrySetResult(null);
        }
    }

    /// <summary>
    /// Resets the event. If the event is already reset, this method does nothing.
    /// </summary>
    public void Reset()
    {
        lock (_mutex)
        {
            if (_tcs.Task.IsCompleted)
            {
                _tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<object?>();
            }
        }
    }

    // ReSharper disable UnusedMember.Local
    [DebuggerNonUserCode]
    private sealed class DebugView
    {
        private readonly AsyncManualResetEvent _mre;

        public DebugView(AsyncManualResetEvent mre)
        {
            _mre = mre;
        }

        public int Id => _mre.Id;

        public bool IsSet => _mre.GetStateForDebugger;

        public Task CurrentTask => _mre._tcs.Task;
    }
    // ReSharper restore UnusedMember.Local
}
