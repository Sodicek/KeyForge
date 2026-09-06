using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Random_password_generator.Resources;

class PasswordGenerator
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        SelectLanguage();

        bool includeCapitalLetters = ReadYesNo(Strings.Get("Prompt_Capitals"));
        bool includeSpecialChars = ReadYesNo(Strings.Get("Prompt_Special"));
        bool includeNumbers = ReadYesNo(Strings.Get("Prompt_Numbers"));

        int minLength = 1 + (includeCapitalLetters ? 1 : 0) + (includeSpecialChars ? 1 : 0) + (includeNumbers ? 1 : 0);
        int length = ReadPositiveInt(Strings.Get("Prompt_Length", minLength), minLength);

        string password = GeneratePassword(length, includeCapitalLetters, includeSpecialChars, includeNumbers);
        Console.WriteLine(Strings.Get("Result_Password", password));

        bool saveToDesktop = ReadYesNo(Strings.Get("Prompt_Save"));
        if (saveToDesktop)
        {
            Console.Write(Strings.Get("Prompt_Directory"));
            string? directoryName = Console.ReadLine();

            SavePasswordToFile(password, string.IsNullOrWhiteSpace(directoryName) ? null : directoryName.Trim());
        }
    }

    static void SelectLanguage()
    {
        Console.Write("Zvolte jazyk / Choose language: [1] Čeština (výchozí/default)  [2] English: ");
        string? choice = Console.ReadLine()?.Trim();

        CultureInfo culture = string.Equals(choice, "2", StringComparison.OrdinalIgnoreCase)
            || string.Equals(choice, "en", StringComparison.OrdinalIgnoreCase)
            ? new CultureInfo("en-US")
            : new CultureInfo("cs-CZ");

        CultureInfo.CurrentUICulture = culture;
    }

    static int ReadPositiveInt(string prompt, int minimum = 1)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= minimum)
            {
                return value;
            }

            Console.WriteLine(Strings.Get("Error_Length", minimum));
        }
    }

    static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();

            if (string.Equals(input, "Y", StringComparison.OrdinalIgnoreCase)
                || string.Equals(input, "A", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(input, "N", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Console.WriteLine(Strings.Get("Error_YesNo"));
        }
    }

    static string GeneratePassword(int length, bool includeCapitalLetters, bool includeSpecialChars, bool includeNumbers)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyz";
        const string capitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "1234567890";
        const string specialChars = "!@#$%^&*()_+-=[]{};:,.<>?";

        var requiredCategories = new System.Collections.Generic.List<string> { letters };

        if (includeCapitalLetters)
        {
            requiredCategories.Add(capitalLetters);
        }

        if (includeSpecialChars)
        {
            requiredCategories.Add(specialChars);
        }

        if (includeNumbers)
        {
            requiredCategories.Add(numbers);
        }

        string charset = string.Concat(requiredCategories);
        char[] password = new char[length];

        // Guarantee at least one character from each selected category.
        for (int i = 0; i < requiredCategories.Count; i++)
        {
            string category = requiredCategories[i];
            password[i] = category[RandomNumberGenerator.GetInt32(category.Length)];
        }

        for (int i = requiredCategories.Count; i < length; i++)
        {
            password[i] = charset[RandomNumberGenerator.GetInt32(charset.Length)];
        }

        Shuffle(password);

        return new string(password);
    }

    static void Shuffle(char[] chars)
    {
        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
    }

    static void SavePasswordToFile(string password, string? directoryName)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string directoryPath = directoryName == null ? desktopPath : Path.Combine(desktopPath, directoryName);

        string fullDirectoryPath = Path.GetFullPath(directoryPath);
        string fullDesktopPath = Path.GetFullPath(desktopPath);

        if (!fullDirectoryPath.StartsWith(fullDesktopPath, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(Strings.Get("Error_Directory"));
            return;
        }

        try
        {
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            string filePath = Path.Combine(fullDirectoryPath, "password.txt");
            File.WriteAllText(filePath, password);
            Console.WriteLine(Strings.Get("Info_Saved", filePath));
        }
        catch (Exception ex)
        {
            Console.WriteLine(Strings.Get("Error_Save", ex.Message));
        }
    }
}
