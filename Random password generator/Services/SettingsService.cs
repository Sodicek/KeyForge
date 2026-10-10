using System.Text.Json;

namespace Random_password_generator.Services;

static class SettingsService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    // Null when the platform has no per-user config folder (e.g. some headless Linux setups).
    public static string? DefaultPath
    {
        get
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return string.IsNullOrEmpty(root) ? null : Path.Combine(root, "KeyForge", "settings.json");
        }
    }

    public static AppSettings Load(string? path = null)
    {
        path ??= DefaultPath;

        if (path is null || !File.Exists(path))
        {
            return new AppSettings();
        }

        try
        {
            AppSettings? loaded = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path));
            return (loaded ?? new AppSettings()).Normalize();
        }
        catch
        {
            // Corrupt or unreadable settings must not stop the app from starting.
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings, string? path = null)
    {
        path ??= DefaultPath;

        if (path is null)
        {
            return;
        }

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, JsonSerializer.Serialize(settings, JsonOptions));
        }
        catch
        {
            // Persisting preferences is best-effort; failing to write must not disturb the user.
        }
    }
}
