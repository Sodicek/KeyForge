using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class PasswordGenerator
{
    static void Main(string[] args)
    {
        int length = ReadPositiveInt("Enter the length of the password: ");
        bool includeCapitalLetters = ReadYesNo("Do you want to include capital letters? (Y/N): ");
        bool includeSpecialChars = ReadYesNo("Do you want to include special characters? (Y/N): ");
        bool includeNumbers = ReadYesNo("Do you want to include numbers? (Y/N): ");

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

    static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Please enter a positive whole number.");
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

        StringBuilder sourceChars = new StringBuilder(letters);

        if (includeCapitalLetters)
        {
            sourceChars.Append(capitalLetters);
        }

        if (includeSpecialChars)
        {
            sourceChars.Append(specialChars);
        }

        if (includeNumbers)
        {
            sourceChars.Append(numbers);
        }

        string charset = sourceChars.ToString();
        StringBuilder password = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int index = RandomNumberGenerator.GetInt32(charset.Length);
            password.Append(charset[index]);
        }

        return password.ToString();
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
