using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Random_password_generator.ViewModels;

namespace Random_password_generator.Views;

public partial class MainWindow : Window
{
    private static readonly TimeSpan ClipboardClearDelay = TimeSpan.FromSeconds(30);

    private CancellationTokenSource? _clipboardClearCts;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnCopyClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm || string.IsNullOrEmpty(vm.GeneratedPassword))
        {
            return;
        }

        IClipboard? clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard is null)
        {
            return;
        }

        string password = vm.GeneratedPassword;
        await clipboard.SetTextAsync(password);
        vm.NotifyPasswordCopied();

        ScheduleClipboardClear(clipboard, password);
    }

    private void ScheduleClipboardClear(IClipboard clipboard, string copiedPassword)
    {
        _clipboardClearCts?.Cancel();
        CancellationTokenSource cts = new();
        _clipboardClearCts = cts;

        _ = ClearClipboardAfterDelay(clipboard, copiedPassword, cts.Token);
    }

    private static async Task ClearClipboardAfterDelay(IClipboard clipboard, string copiedPassword, CancellationToken token)
    {
        try
        {
            await Task.Delay(ClipboardClearDelay, token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        if (token.IsCancellationRequested)
        {
            return;
        }

        string? current = await clipboard.TryGetTextAsync();
        if (current == copiedPassword)
        {
            await clipboard.SetTextAsync(string.Empty);
        }
    }
}
