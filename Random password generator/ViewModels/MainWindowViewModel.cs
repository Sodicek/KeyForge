using System.Globalization;
using System.Reflection;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Random_password_generator.Resources;
using Random_password_generator.Services;

namespace Random_password_generator.ViewModels;

public enum PasswordStrength
{
    VeryWeak,
    Weak,
    Medium,
    Strong,
    VeryStrong,
}

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
    private bool excludeAmbiguous;

    [ObservableProperty]
    private double length = 16;

    [ObservableProperty]
    private bool usePassphraseMode;

    [ObservableProperty]
    private double passphraseWordCount = 5;

    [ObservableProperty]
    private bool passphraseCapitalize = true;

    [ObservableProperty]
    private bool passphraseAppendNumber;

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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StrengthText))]
    [NotifyPropertyChangedFor(nameof(StrengthColor))]
    [NotifyPropertyChangedFor(nameof(StrengthPercent))]
    private double entropyBits;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StrengthText))]
    [NotifyPropertyChangedFor(nameof(StrengthColor))]
    private PasswordStrength strength;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UpdateMessage))]
    [NotifyPropertyChangedFor(nameof(IsUpdateAvailable))]
    private string? availableUpdateVersion;

    public double MinLength => PasswordService.MinLength(IncludeCapitals, IncludeSpecial, IncludeNumbers);

    public double StrengthPercent => Math.Clamp(EntropyBits / 1.28, 0, 100);

    public bool IsUpdateAvailable => AvailableUpdateVersion is not null;

    public string Title => Strings.Get("App_Title");
    public string CapitalsLabel => Strings.Get("Label_Capitals");
    public string SpecialLabel => Strings.Get("Label_Special");
    public string NumbersLabel => Strings.Get("Label_Numbers");
    public string ExcludeAmbiguousLabel => Strings.Get("Label_ExcludeAmbiguous");
    public string LengthLabel => Strings.Get("Label_Length");
    public string GenerateLabel => Strings.Get("Button_Generate");
    public string CopyLabel => Strings.Get("Button_Copy");
    public string SaveLabel => Strings.Get("Button_Save");
    public string SaveToDesktopLabel => Strings.Get("Label_SaveToDesktop");
    public string DirectoryPlaceholder => Strings.Get("Placeholder_Directory");
    public string ModeCharactersLabel => Strings.Get("Label_ModeCharacters");
    public string ModePassphraseLabel => Strings.Get("Label_ModePassphrase");
    public string WordCountLabel => Strings.Get("Label_WordCount");
    public string CapitalizeLabel => Strings.Get("Label_Capitalize");
    public string AppendNumberLabel => Strings.Get("Label_AppendNumber");

    public string StrengthText => Strings.Get("Label_Strength", Strings.Get(StrengthResourceKey(Strength)), EntropyBits);

    public string? UpdateMessage => AvailableUpdateVersion is null
        ? null
        : Strings.Get("Status_UpdateAvailable", AvailableUpdateVersion);

    public IBrush StrengthColor => Strength switch
    {
        PasswordStrength.VeryWeak => new SolidColorBrush(Color.Parse("#E74C3C")),
        PasswordStrength.Weak => new SolidColorBrush(Color.Parse("#E67E22")),
        PasswordStrength.Medium => new SolidColorBrush(Color.Parse("#F1C40F")),
        PasswordStrength.Strong => new SolidColorBrush(Color.Parse("#2ECC71")),
        _ => new SolidColorBrush(Color.Parse("#27AE60")),
    };

    public MainWindowViewModel()
    {
        Strings.Culture = new CultureInfo("cs-CZ");
        ApplySettings(SettingsService.Load());
        Generate();
        _ = CheckForUpdatesAsync();
    }

    public void SaveSettings()
    {
        SettingsService.Save(new AppSettings
        {
            LanguageIndex = LanguageIndex,
            UsePassphraseMode = UsePassphraseMode,
            IncludeCapitals = IncludeCapitals,
            IncludeSpecial = IncludeSpecial,
            IncludeNumbers = IncludeNumbers,
            ExcludeAmbiguous = ExcludeAmbiguous,
            Length = Length,
            PassphraseWordCount = PassphraseWordCount,
            PassphraseCapitalize = PassphraseCapitalize,
            PassphraseAppendNumber = PassphraseAppendNumber,
            SaveFolderName = SaveFolderName,
        });
    }

    private void ApplySettings(AppSettings settings)
    {
        LanguageIndex = settings.LanguageIndex;
        UsePassphraseMode = settings.UsePassphraseMode;
        IncludeCapitals = settings.IncludeCapitals;
        IncludeSpecial = settings.IncludeSpecial;
        IncludeNumbers = settings.IncludeNumbers;
        ExcludeAmbiguous = settings.ExcludeAmbiguous;
        Length = settings.Length;
        PassphraseWordCount = settings.PassphraseWordCount;
        PassphraseCapitalize = settings.PassphraseCapitalize;
        PassphraseAppendNumber = settings.PassphraseAppendNumber;
        SaveFolderName = settings.SaveFolderName;
    }

    partial void OnLanguageIndexChanged(int value)
    {
        Strings.Culture = value == 1 ? new CultureInfo("en-US") : new CultureInfo("cs-CZ");
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
        if (UsePassphraseMode)
        {
            int words = (int)PassphraseWordCount;
            GeneratedPassword = PassphraseService.Generate(words, PassphraseCapitalize, PassphraseAppendNumber);
            EntropyBits = PassphraseService.EntropyBits(words, PassphraseAppendNumber);
        }
        else
        {
            if (Length < MinLength)
            {
                Length = MinLength;
            }

            int length = (int)Length;
            GeneratedPassword = PasswordService.Generate(length, IncludeCapitals, IncludeSpecial, IncludeNumbers, ExcludeAmbiguous);

            int charsetSize = PasswordService.CharsetSize(IncludeCapitals, IncludeSpecial, IncludeNumbers, ExcludeAmbiguous);
            EntropyBits = length * Math.Log2(charsetSize);
        }

        Strength = ClassifyStrength(EntropyBits);
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

    [RelayCommand]
    private void OpenUpdatePage()
    {
        UrlOpener.Open(UpdateCheckService.ReleasesPageUrl);
    }

    public void NotifyPasswordCopied()
    {
        StatusMessage = Strings.Get("Status_CopiedWithClear");
    }

    private async Task CheckForUpdatesAsync()
    {
        Version current = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0);
        AvailableUpdateVersion = await UpdateCheckService.GetNewerVersionAsync(current);
    }

    private static PasswordStrength ClassifyStrength(double bits) => bits switch
    {
        < 28 => PasswordStrength.VeryWeak,
        < 36 => PasswordStrength.Weak,
        < 60 => PasswordStrength.Medium,
        < 90 => PasswordStrength.Strong,
        _ => PasswordStrength.VeryStrong,
    };

    private static string StrengthResourceKey(PasswordStrength strength) => strength switch
    {
        PasswordStrength.VeryWeak => "Strength_VeryWeak",
        PasswordStrength.Weak => "Strength_Weak",
        PasswordStrength.Medium => "Strength_Medium",
        PasswordStrength.Strong => "Strength_Strong",
        _ => "Strength_VeryStrong",
    };

    private void RefreshLocalizedText()
    {
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(CapitalsLabel));
        OnPropertyChanged(nameof(SpecialLabel));
        OnPropertyChanged(nameof(NumbersLabel));
        OnPropertyChanged(nameof(ExcludeAmbiguousLabel));
        OnPropertyChanged(nameof(LengthLabel));
        OnPropertyChanged(nameof(GenerateLabel));
        OnPropertyChanged(nameof(CopyLabel));
        OnPropertyChanged(nameof(SaveLabel));
        OnPropertyChanged(nameof(SaveToDesktopLabel));
        OnPropertyChanged(nameof(DirectoryPlaceholder));
        OnPropertyChanged(nameof(StrengthText));
        OnPropertyChanged(nameof(ModeCharactersLabel));
        OnPropertyChanged(nameof(ModePassphraseLabel));
        OnPropertyChanged(nameof(WordCountLabel));
        OnPropertyChanged(nameof(CapitalizeLabel));
        OnPropertyChanged(nameof(AppendNumberLabel));
        OnPropertyChanged(nameof(UpdateMessage));
    }
}
