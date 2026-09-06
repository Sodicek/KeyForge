using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Random_password_generator.Resources;
using Random_password_generator.Services;

namespace Random_password_generator.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MinLength))]
    private bool includeCapitals = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MinLength))]
    private bool includeSpecial = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MinLength))]
    private bool includeNumbers = true;

    [ObservableProperty]
    private double length = 16;

    [ObservableProperty]
    private string generatedPassword = string.Empty;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private bool saveToDesktop;

    [ObservableProperty]
    private string? saveFolderName;

    [ObservableProperty]
    private int languageIndex;

    public double MinLength => PasswordService.MinLength(IncludeCapitals, IncludeSpecial, IncludeNumbers);

    public string Title => Strings.Get("App_Title");
    public string CapitalsLabel => Strings.Get("Label_Capitals");
    public string SpecialLabel => Strings.Get("Label_Special");
    public string NumbersLabel => Strings.Get("Label_Numbers");
    public string LengthLabel => Strings.Get("Label_Length");
    public string GenerateLabel => Strings.Get("Button_Generate");
    public string CopyLabel => Strings.Get("Button_Copy");
    public string SaveLabel => Strings.Get("Button_Save");
    public string SaveToDesktopLabel => Strings.Get("Label_SaveToDesktop");
    public string DirectoryPlaceholder => Strings.Get("Placeholder_Directory");

    public MainWindowViewModel()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("cs-CZ");
        Generate();
    }

    partial void OnLanguageIndexChanged(int value)
    {
        CultureInfo.CurrentUICulture = value == 1 ? new CultureInfo("en-US") : new CultureInfo("cs-CZ");
        RefreshLocalizedText();
    }

    partial void OnLengthChanged(double value)
    {
        if (value < MinLength)
        {
            Length = MinLength;
        }
    }

    [RelayCommand]
    private void Generate()
    {
        if (Length < MinLength)
        {
            Length = MinLength;
        }

        GeneratedPassword = PasswordService.Generate((int)Length, IncludeCapitals, IncludeSpecial, IncludeNumbers);
        StatusMessage = string.Empty;
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrEmpty(GeneratedPassword))
        {
            return;
        }

        string? folder = string.IsNullOrWhiteSpace(SaveFolderName) ? null : SaveFolderName.Trim();
        (_, string message) = FileSaveService.Save(GeneratedPassword, folder);
        StatusMessage = message;
    }

    public void NotifyPasswordCopied()
    {
        StatusMessage = Strings.Get("Status_Copied");
    }

    private void RefreshLocalizedText()
    {
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(CapitalsLabel));
        OnPropertyChanged(nameof(SpecialLabel));
        OnPropertyChanged(nameof(NumbersLabel));
        OnPropertyChanged(nameof(LengthLabel));
        OnPropertyChanged(nameof(GenerateLabel));
        OnPropertyChanged(nameof(CopyLabel));
        OnPropertyChanged(nameof(SaveLabel));
        OnPropertyChanged(nameof(SaveToDesktopLabel));
        OnPropertyChanged(nameof(DirectoryPlaceholder));
    }
}
