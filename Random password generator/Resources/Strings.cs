using System.Globalization;
using System.Resources;

namespace Random_password_generator.Resources;

static class Strings
{
    private static readonly ResourceManager ResourceManager =
        new("Random_password_generator.Resources.Strings", typeof(Strings).Assembly);

    public static string Get(string key, params object[] args)
    {
        string format = ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        return args.Length == 0 ? format : string.Format(CultureInfo.CurrentUICulture, format, args);
    }
}
