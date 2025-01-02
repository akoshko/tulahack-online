using System;

namespace Tulahack.UI.Validation.Resources.StringSources;

/// <summary>
/// String source which return message of exception.
/// </summary>
public class ExceptionSource : IStringSource
{
    private readonly Exception _exception;

    /// <summary>
    /// Create new instance of exception source.
    /// </summary>
    /// <param name="exception">Exception.</param>
    public ExceptionSource(Exception exception)
    {
        _exception = exception;
    }

    /// <inheritdoc />
    public string GetString() =>
        _exception.Message;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((ExceptionSource)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        _exception.GetHashCode();

    /// <summary>
    /// Check if two sources are equal.
    /// </summary>
    protected bool Equals(ExceptionSource other) =>
        Equals(_exception, other._exception);
}
