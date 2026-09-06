using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class PasswordGenerator
{
    static void Main(string[] args)
    {
        bool includeCapitalLetters = ReadYesNo("Do you want to include capital letters? (Y/N): ");
        bool includeSpecialChars = ReadYesNo("Do you want to include special characters? (Y/N): ");
        bool includeNumbers = ReadYesNo("Do you want to include numbers? (Y/N): ");

        int minLength = 1 + (includeCapitalLetters ? 1 : 0) + (includeSpecialChars ? 1 : 0) + (includeNumbers ? 1 : 0);
        int length = ReadPositiveInt($"Enter the length of the password (minimum {minLength} for the selected options): ", minLength);

        string password = GeneratePassword(length, includeCapitalLetters, includeSpecialChars, includeNumbers);
        Console.WriteLine("Your password is: " + password);

        bool saveToDesktop = ReadYesNo("Do you want to save the password to the desktop? (Y/N): ");
        if (saveToDesktop)
        {
            Console.Write("Enter the directory name where you want to save the password file (leave empty for Desktop root): ");
            string? directoryName = Console.ReadLine();

            SavePasswordToFile(password, string.IsNullOrWhiteSpace(directoryName) ? null : directoryName.Trim());
        }
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

            Console.WriteLine($"Please enter a whole number of at least {minimum}.");
        }
    }

    static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();

            if (string.Equals(input, "Y", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(input, "N", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Console.WriteLine("Please answer with Y or N.");
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
            Console.WriteLine("Invalid directory name.");
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
            Console.WriteLine("Password saved to file: " + filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving password to file: " + ex.Message);
        }
    }
}
