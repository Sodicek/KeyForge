namespace Random_password_generator.Services;

// Words come from the EFF Large Wordlist (CC BY 3.0 US), embedded as "wordlist.txt".
static class WordList
{
    public static readonly string[] Words = Load();

    private static string[] Load()
    {
        using Stream stream = typeof(WordList).Assembly.GetManifestResourceStream("wordlist.txt")
            ?? throw new InvalidOperationException("Embedded word list 'wordlist.txt' was not found.");
        using StreamReader reader = new(stream);

        return reader.ReadToEnd()
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct()
            .ToArray();
    }
}
