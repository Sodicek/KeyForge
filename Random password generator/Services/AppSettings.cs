namespace Random_password_generator.Services;

class AppSettings
{
    public int LanguageIndex { get; set; }
    public bool UsePassphraseMode { get; set; }
    public bool IncludeCapitals { get; set; } = true;
    public bool IncludeSpecial { get; set; } = true;
    public bool IncludeNumbers { get; set; } = true;
    public bool ExcludeAmbiguous { get; set; }
    public double Length { get; set; } = 16;
    public double PassphraseWordCount { get; set; } = 5;
    public bool PassphraseCapitalize { get; set; } = true;
    public bool PassphraseAppendNumber { get; set; }
    public string? SaveFolderName { get; set; }

    // A hand-edited or outdated file must never put the UI into an invalid state.
    public AppSettings Normalize()
    {
        LanguageIndex = Math.Clamp(LanguageIndex, 0, 1);
        Length = Math.Clamp(Length, 1, 64);
        PassphraseWordCount = Math.Clamp(PassphraseWordCount, 3, 8);
        return this;
    }
}
