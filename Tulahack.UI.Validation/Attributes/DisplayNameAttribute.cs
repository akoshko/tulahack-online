﻿using System;
using Tulahack.UI.Validation.Options;
using Tulahack.UI.Validation.Resources.StringProviders;

namespace Tulahack.UI.Validation.Attributes;

/// <summary>
/// Attribute which allow set display name of property.
/// Display name will be used in validation messages.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class DisplayNameAttribute : Attribute
{
    /// <summary>
    /// Display name for property.
    /// </summary>
    public string? Name { get; set; } = string.Empty;

    /// <summary>
    /// Key of display name for <see cref="IStringProvider" /> in <see cref="ValidationOptions.LanguageManager" />.
    /// </summary>
    public string? DisplayNameKey { get; set; }

    /// <summary>
    /// Name of resource for <see cref="IStringProvider" /> in <see cref="ValidationOptions.LanguageManager" />.
    /// </summary>
    public string? DisplayNameResource { get; set; }


    /// <summary>
    /// Get display name of property.
    /// </summary>
    /// <returns>Localized display name.</returns>
    public string GetDisplayName()
    {
        if (!string.IsNullOrEmpty(DisplayNameKey))
        {
            return ValidationOptions.LanguageManager.GetString(DisplayNameKey!, DisplayNameResource);
        }

        return Name ?? string.Empty;
    }
}
