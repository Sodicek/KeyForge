using System.Globalization;
using System.Resources;

namespace Random_password_generator.Resources;

static class Strings
{
    private static readonly ResourceManager ResourceManager =
        new("Random_password_generator.Resources.Strings", typeof(Strings).Assembly);

    // Explicit, not thread-static: the active language must stay correct even when a
    // localized string is produced from an async continuation that may resume on a
    // different thread than the one that set the language.
    public static CultureInfo Culture { get; set; } = CultureInfo.CurrentUICulture;

    public static string Get(string key, params object[] args)
    {
        string format = ResourceManager.GetString(key, Culture) ?? key;
        return args.Length == 0 ? format : string.Format(Culture, format, args);
    }
}
