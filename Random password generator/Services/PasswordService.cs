using System.Security.Cryptography;

namespace Random_password_generator.Services;

static class PasswordService
{
    private const string Letters = "abcdefghijklmnopqrstuvwxyz";
    private const string CapitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "1234567890";
    private const string SpecialChars = "!@#$%^&*()_+-=[]{};:,.<>?";
    private const string AmbiguousChars = "0O1lI";

    public static int MinLength(bool includeCapitals, bool includeSpecial, bool includeNumbers)
        => 1 + (includeCapitals ? 1 : 0) + (includeSpecial ? 1 : 0) + (includeNumbers ? 1 : 0);

    public static int CharsetSize(bool includeCapitals, bool includeSpecial, bool includeNumbers, bool excludeAmbiguous)
        => BuildCategories(includeCapitals, includeSpecial, includeNumbers, excludeAmbiguous).Sum(c => c.Length);

    public static string Generate(int length, bool includeCapitals, bool includeSpecial, bool includeNumbers, bool excludeAmbiguous)
    {
        List<string> categories = BuildCategories(includeCapitals, includeSpecial, includeNumbers, excludeAmbiguous);
        string charset = string.Concat(categories);
        char[] password = new char[length];

        // Guarantee at least one character from each selected category.
        for (int i = 0; i < categories.Count; i++)
        {
            string category = categories[i];
            password[i] = category[RandomNumberGenerator.GetInt32(category.Length)];
        }

        for (int i = categories.Count; i < length; i++)
        {
            password[i] = charset[RandomNumberGenerator.GetInt32(charset.Length)];
        }

        Shuffle(password);

        return new string(password);
    }

    private static List<string> BuildCategories(bool includeCapitals, bool includeSpecial, bool includeNumbers, bool excludeAmbiguous)
    {
        var categories = new List<string> { Filter(Letters, excludeAmbiguous) };

        if (includeCapitals)
        {
            categories.Add(Filter(CapitalLetters, excludeAmbiguous));
        }

        if (includeSpecial)
        {
            categories.Add(Filter(SpecialChars, excludeAmbiguous));
        }

        if (includeNumbers)
        {
            categories.Add(Filter(Numbers, excludeAmbiguous));
        }

        return categories;
    }

    private static string Filter(string source, bool excludeAmbiguous)
        => excludeAmbiguous ? new string(source.Where(c => !AmbiguousChars.Contains(c)).ToArray()) : source;

    private static void Shuffle(char[] chars)
    {
        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
    }
}
