using Random_password_generator.Services;

namespace KeyForge.Tests;

public class SettingsServiceTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "keyforge-tests-" + Guid.NewGuid().ToString("N"));

    private string SettingsPath => Path.Combine(_directory, "nested", "settings.json");

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }

    [Fact]
    public void Load_WhenFileIsMissing_ReturnsDefaults()
    {
        AppSettings settings = SettingsService.Load(SettingsPath);

        Assert.Equal(16, settings.Length);
        Assert.Equal(5, settings.PassphraseWordCount);
        Assert.True(settings.IncludeCapitals);
        Assert.False(settings.UsePassphraseMode);
    }

    [Fact]
    public void SaveThenLoad_RoundTripsAllValues()
    {
        var original = new AppSettings
        {
            LanguageIndex = 1,
            UsePassphraseMode = true,
            IncludeCapitals = false,
            IncludeSpecial = false,
            IncludeNumbers = false,
            ExcludeAmbiguous = true,
            Length = 32,
            PassphraseWordCount = 7,
            PassphraseCapitalize = false,
            PassphraseAppendNumber = true,
            SaveFolderName = "vault",
        };

        SettingsService.Save(original, SettingsPath);
        AppSettings loaded = SettingsService.Load(SettingsPath);

        Assert.Equal(1, loaded.LanguageIndex);
        Assert.True(loaded.UsePassphraseMode);
        Assert.False(loaded.IncludeCapitals);
        Assert.False(loaded.IncludeSpecial);
        Assert.False(loaded.IncludeNumbers);
        Assert.True(loaded.ExcludeAmbiguous);
        Assert.Equal(32, loaded.Length);
        Assert.Equal(7, loaded.PassphraseWordCount);
        Assert.False(loaded.PassphraseCapitalize);
        Assert.True(loaded.PassphraseAppendNumber);
        Assert.Equal("vault", loaded.SaveFolderName);
    }

    [Fact]
    public void Load_WithCorruptFile_ReturnsDefaultsInsteadOfThrowing()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
        File.WriteAllText(SettingsPath, "{ this is not json");

        AppSettings settings = SettingsService.Load(SettingsPath);

        Assert.Equal(16, settings.Length);
    }

    [Fact]
    public void Load_ClampsOutOfRangeValues()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
        File.WriteAllText(SettingsPath, """{ "LanguageIndex": 9, "Length": 5000, "PassphraseWordCount": 0 }""");

        AppSettings settings = SettingsService.Load(SettingsPath);

        Assert.Equal(1, settings.LanguageIndex);
        Assert.Equal(64, settings.Length);
        Assert.Equal(3, settings.PassphraseWordCount);
    }

    [Fact]
    public void SavedFile_NeverContainsAPassword()
    {
        SettingsService.Save(new AppSettings(), SettingsPath);

        string json = File.ReadAllText(SettingsPath);

        Assert.DoesNotContain("password", json, StringComparison.OrdinalIgnoreCase);
    }
}
