using System;
using System.Linq.Expressions;
using System.Reflection;
using Tulahack.UI.Validation.Attributes;
using Tulahack.UI.Validation.Helpers;
using Tulahack.UI.Validation.Resources.StringSources;

namespace Tulahack.UI.Validation.Resources.DisplayNameResolver;

/// <summary>
/// Display name resolver, which uses <see cref="DisplayNameAttribute" />.
/// </summary>
internal class DefaultDisplayNameResolver : IDisplayNameResolver
{
    /// <inheritdoc />
    public IStringSource? GetPropertyNameSource(PropertyInfo propertyInfo)
    {
        var displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
        return displayNameAttribute == null
            ? null
            : new DisplayNamePropertySource(displayNameAttribute);
    }

    /// <inheritdoc />
    public IStringSource? GetPropertyNameSource(Type objectType, LambdaExpression expression)
    {
        var propertyName = ReactiveValidationHelper.GetPropertyName(objectType, expression);
        if (string.IsNullOrEmpty(propertyName))
        {
            return null;
        }

        var displayNameAttribute = objectType.GetProperty(propertyName)?.GetCustomAttribute<DisplayNameAttribute>();
        return displayNameAttribute == null
            ? null
            : new DisplayNamePropertySource(displayNameAttribute);
    }
}