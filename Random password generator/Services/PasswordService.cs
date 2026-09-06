using System.Security.Cryptography;

namespace Random_password_generator.Services;

static class PasswordService
{
    private const string Letters = "abcdefghijklmnopqrstuvwxyz";
    private const string CapitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "1234567890";
    private const string SpecialChars = "!@#$%^&*()_+-=[]{};:,.<>?";

    public static int MinLength(bool includeCapitals, bool includeSpecial, bool includeNumbers)
        => 1 + (includeCapitals ? 1 : 0) + (includeSpecial ? 1 : 0) + (includeNumbers ? 1 : 0);

    public static string Generate(int length, bool includeCapitals, bool includeSpecial, bool includeNumbers)
    {
        var categories = new List<string> { Letters };

        if (includeCapitals)
        {
            categories.Add(CapitalLetters);
        }

        if (includeSpecial)
        {
            categories.Add(SpecialChars);
        }

        if (includeNumbers)
        {
            categories.Add(Numbers);
        }

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

    private static void Shuffle(char[] chars)
    {
        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
    }
}
