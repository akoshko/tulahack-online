﻿using System;
using Tulahack.UI.Validation.Attributes;

namespace Tulahack.UI.Validation.Resources.StringSources;

/// <summary>
/// String source which returns display name of property.
/// </summary>
public class DisplayNamePropertySource : IStringSource
{
    private readonly DisplayNameAttribute _displayNameAttribute;

    /// <summary>
    /// Create new instance of display name property source.
    /// </summary>
    /// <param name="displayNameAttribute">Attribute for display name.</param>
    public DisplayNamePropertySource(DisplayNameAttribute displayNameAttribute)
    {
        _displayNameAttribute = displayNameAttribute ?? throw new ArgumentNullException(nameof(displayNameAttribute));
    }

    /// <inheritdoc />
    public string GetString() =>
        _displayNameAttribute.GetDisplayName();

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

        return Equals((DisplayNamePropertySource)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        _displayNameAttribute.GetHashCode();

    /// <summary>
    /// Check if two sources are equal.
    /// </summary>
    protected bool Equals(DisplayNamePropertySource other) =>
        Equals(_displayNameAttribute, other._displayNameAttribute);
}
