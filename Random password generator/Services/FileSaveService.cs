using Random_password_generator.Resources;

namespace Random_password_generator.Services;

static class FileSaveService
{
    public static (bool Success, string Message) Save(string password, string? directoryName)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string directoryPath = directoryName == null ? desktopPath : Path.Combine(desktopPath, directoryName);

        string fullDirectoryPath = Path.GetFullPath(directoryPath);
        string fullDesktopPath = Path.GetFullPath(desktopPath);

        if (!fullDirectoryPath.StartsWith(fullDesktopPath, StringComparison.OrdinalIgnoreCase))
        {
            return (false, Strings.Get("Status_InvalidDirectory"));
        }

        try
        {
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            string filePath = Path.Combine(fullDirectoryPath, "password.txt");
            File.WriteAllText(filePath, password);
            return (true, Strings.Get("Status_Saved", filePath));
        }
        catch (Exception ex)
        {
            return (false, Strings.Get("Status_SaveError", ex.Message));
        }
    }
}
