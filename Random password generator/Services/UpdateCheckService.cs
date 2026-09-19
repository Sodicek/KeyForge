using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Random_password_generator.Services;

static class UpdateCheckService
{
    private const string LatestReleaseApiUrl = "https://api.github.com/repos/Sodicek/KeyForge/releases/latest";
    public const string ReleasesPageUrl = "https://github.com/Sodicek/KeyForge/releases/latest";

    public static async Task<string?> GetNewerVersionAsync(Version currentVersion)
    {
        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("KeyForge", currentVersion.ToString()));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            client.Timeout = TimeSpan.FromSeconds(5);

            using Stream stream = await client.GetStreamAsync(LatestReleaseApiUrl);
            using JsonDocument doc = await JsonDocument.ParseAsync(stream);

            string? tagName = doc.RootElement.GetProperty("tag_name").GetString();
            string? versionText = tagName?.TrimStart('v', 'V');

            if (versionText is null || !Version.TryParse(versionText, out Version? latestVersion))
            {
                return null;
            }

            // Compare only Major.Minor.Build — Revision is meaningless for our version scheme
            // and a missing component parses as -1, which would otherwise skew the comparison.
            Version normalizedLatest = new(latestVersion.Major, latestVersion.Minor, Math.Max(latestVersion.Build, 0));
            Version normalizedCurrent = new(currentVersion.Major, currentVersion.Minor, Math.Max(currentVersion.Build, 0));

            return normalizedLatest > normalizedCurrent ? versionText : null;
        }
        catch
        {
            // Offline, rate-limited, or GitHub unreachable: silently skip the check.
            return null;
        }
    }
}
