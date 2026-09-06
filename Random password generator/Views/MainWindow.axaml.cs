using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Random_password_generator.ViewModels;

namespace Random_password_generator.Views;

public partial class MainWindow : Window
{
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

        await clipboard.SetTextAsync(vm.GeneratedPassword);
        vm.NotifyPasswordCopied();
    }
}
