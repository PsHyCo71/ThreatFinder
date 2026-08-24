using System.Text.Json;
using ThreatFinder.Core;
using ThreatFinder.Providers;

namespace ThreatFinder.Tests;

public class MBProviderIntegrationTests
{
    private static string FindAppSettingsPath()
    {
        DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir is not null)
        {
            string candidate = Path.Combine(dir.FullName, "appsettings.json");
            if (File.Exists(candidate))
                return candidate;

            dir = dir.Parent;
        }

        throw new FileNotFoundException("appsettings.json non trovato risalendo da " + AppContext.BaseDirectory);
    }

    private static async Task<string> GetMalwareBazaarApiKeyAsync()
    {
        string path = FindAppSettingsPath();
        string json = await File.ReadAllTextAsync(path);
        AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(json);

        return settings?.MalwareBazaarApiKey
            ?? throw new InvalidOperationException("MalwareBazaarApiKey non impostata in appsettings.json");
    }

    [Fact]
    public async Task CheckFileHashAsync_WithKnownMaliciousHash_ReturnsMaliciousResult()
    {
        string apiKey = await GetMalwareBazaarApiKeyAsync();

        var httpClient = new HttpClient();
        var provider = new MBProvider(httpClient);

        EngineResult result = await provider.CheckFileHashAsync(
            apiKey,
            "eb955440673c18e0c82ad3d485d5417013cfe0caa37d8528bfe153c80291e65b");

        var malicious = Assert.IsType<MaliciousResult>(result);
        Assert.Equal("MalwareBazaar", malicious.ProviderName);
        Assert.False(string.IsNullOrWhiteSpace(malicious.ThreatType));
    }
}