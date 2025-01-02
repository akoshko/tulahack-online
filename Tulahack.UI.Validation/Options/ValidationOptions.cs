using ReactiveValidation;
using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Exceptions;
using Tulahack.UI.Validation.Resources.DisplayNameResolver;
using Tulahack.UI.Validation.ValidatorFactory;

namespace Tulahack.UI.Validation.Options;

/// <summary>
/// Options for validation.
/// </summary>
public static class ValidationOptions
{
    private static bool _isConfigured;

    /// <summary>
    /// Specifies how rules of property should cascade when one fails.
    /// </summary>
    public static CascadeMode PropertyCascadeMode { get; internal set; } = CascadeMode.Continue;

    /// <summary>
    /// Manager, which allows use different localization for validation messages.
    /// </summary>
    public static LanguageManager LanguageManager { get; } = new LanguageManager();

    /// <summary>
    /// Class for resolving display name of properties by its metadata.
    /// </summary>
    public static IDisplayNameResolver DisplayNameResolver { get; internal set; } = new DefaultDisplayNameResolver();

    /// <summary>
    /// Factory for creating validator for object.
    /// </summary>
    public static IValidatorFactory ValidatorFactory { get; internal set; } = new DefaultValidatorFactory();

    /// <summary>
    /// Setup validation options.
    /// </summary>
    /// <exception cref="MethodAlreadyCalledException">Throws if options already was configured.</exception>
    public static ValidationOptionsBuilder Setup()
    {
        if (_isConfigured)
        {
            throw new MethodAlreadyCalledException("Cannot setup options twice");
        }

        _isConfigured = true;
        return new ValidationOptionsBuilder();
    }
}