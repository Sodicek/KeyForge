using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

        AddHandler(KeyDownEvent, OnWindowKeyDown, RoutingStrategies.Tunnel);

        Closing += (_, _) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.SaveSettings();
            }
        };
    }

    private void OnWindowKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && e.KeyModifiers == KeyModifiers.None)
        {
            // Enter keeps its normal meaning on real buttons, text fields and combo boxes; elsewhere it generates.
            // CheckBox/RadioButton derive from Button in Avalonia, so they're excluded from the "real button" check.
            object? focused = FocusManager?.GetFocusedElement();
            bool keepsOwnEnter = (focused is Button && focused is not Avalonia.Controls.Primitives.ToggleButton)
                || focused is TextBox or ComboBox or ComboBoxItem;

            if (!keepsOwnEnter && DataContext is MainWindowViewModel generateVm)
            {
                e.Handled = true;
                generateVm.GenerateCommand.Execute(null);
            }

            return;
        }

        KeyModifiers commandModifier = Application.Current?.PlatformSettings?.HotkeyConfiguration.CommandModifiers ?? KeyModifiers.Control;

        if (e.Key != Key.C || e.KeyModifiers != commandModifier)
        {
            return;
        }

        // Inside an editable field (or with text selected in the password box) the native copy must win.
        if (FocusManager?.GetFocusedElement() is TextBox box && !(box.IsReadOnly && string.IsNullOrEmpty(box.SelectedText)))
        {
            return;
        }

        e.Handled = true;
        _ = CopyPasswordAsync();
    }

    private async void OnCopyClick(object? sender, RoutedEventArgs e) => await CopyPasswordAsync();

    private async Task CopyPasswordAsync()
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
