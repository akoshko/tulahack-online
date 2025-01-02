using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Tulahack.UI.Messaging;
using Tulahack.UI.Utils;

namespace Tulahack.UI.ViewModels.Base;

public abstract partial class ViewModelBase() : ObservableRecipient(WeakReferenceMessenger.Default), IPageContext
{
    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' " +
        "require dynamic access otherwise can break functionality " +
        "when trimming application code",
        Justification = "IsActive is part of CommunityToolkit.Mvvm.ComponentModel")]
    public void Activate() => IsActive = true;

    [UnconditionalSuppressMessage("Trimming",
        "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' " +
        "require dynamic access otherwise can break functionality " +
        "when trimming application code",
        Justification = "IsActive is part of CommunityToolkit.Mvvm.ComponentModel")]
    public void Deactivate()
    {
        IsActive = false;
        NavigationArgs = null;
    }

    public NavigationArgs? NavigationArgs { get; set; }

    public bool CanGoBack => NavigationArgs?.Sender is not null;

    public void SetArgs(NavigationArgs? args) => NavigationArgs = args;

    [RelayCommand]
    public virtual void GoBack(IPageContext? previousPage = null)
    {
        if (previousPage is not null)
        {
            WeakReferenceMessenger.Default.Send(new SelectedPageContextChanged(new SelectedPageChangedArgs
                { ContextType = previousPage.GetType() }));
        }

        if (NavigationArgs?.Sender is not null)
        {
            WeakReferenceMessenger.Default.Send(new SelectedPageContextChanged(new SelectedPageChangedArgs
                { ContextType = NavigationArgs.Sender.GetType() }));
        }
    }

    [RelayCommand]
    public void OpenUrl(object urlObj)
    {
        var url = urlObj as string;
        if (url is null)
        {
            return;
        }

        if (urlObj is Uri uri && !string.IsNullOrEmpty(uri.AbsolutePath))
        {
            url = uri.AbsolutePath;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            //https://stackoverflow.com/a/2796367/241446
            using var proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = url;
            proc.Start();

            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("x-www-browser", url);
            return;
        }

        if (OperatingSystem.IsBrowser())
        {
            new JsExportedMethods().OpenUrl(url);
            return;
        }

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            throw new ArgumentException("invalid url: " + url);
        }

        Process.Start("open", url);
    }
}