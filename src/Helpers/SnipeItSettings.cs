using System.Text.Json;

namespace SnipeITDemo.Helpers;

public static class SnipeItSettings
{
    private const string ConfigFileName = "SnipeIt.config.json";

    private static readonly SnipeItConfig Config = LoadConfig();

    public static string BaseUrl { get; } = Get("SNIPEIT_BASE_URL", Config.BaseUrl).TrimEnd('/');

    public static Account Admin { get; } = new(
        Get("SNIPEIT_ADMIN_USERNAME", Config.Accounts.Admin.Username),
        Get("SNIPEIT_ADMIN_PASSWORD", Config.Accounts.Admin.Password),
        Get("SNIPEIT_ADMIN_DISPLAY_NAME", Config.Accounts.Admin.DisplayName));

    public static Account DemoUser { get; } = new(
        Get("SNIPEIT_USER_USERNAME", Config.Accounts.DemoUser.Username),
        Get("SNIPEIT_USER_PASSWORD", Config.Accounts.DemoUser.Password),
        Get("SNIPEIT_USER_DISPLAY_NAME", Config.Accounts.DemoUser.DisplayName));

    public static NewAsset NewAsset { get; } = new(
        Get("SNIPEIT_ASSET_MODEL", Config.NewAsset.Model),
        Get("SNIPEIT_ASSET_STATUS", Config.NewAsset.Status));

    public static string Url(string path) => $"{BaseUrl}/{path.TrimStart('/')}";

    private static string Get(string environmentVariable, string configValue)
    {
        var value = Environment.GetEnvironmentVariable(environmentVariable);
        return string.IsNullOrWhiteSpace(value) ? configValue : value;
    }

    private static SnipeItConfig LoadConfig()
    {
        var path = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"{ConfigFileName} was not found next to the test assembly. Rebuild the project so it is copied.", path);
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReadCommentHandling = JsonCommentHandling.Skip };
        return JsonSerializer.Deserialize<SnipeItConfig>(File.ReadAllText(path), options)
            ?? throw new InvalidOperationException($"{ConfigFileName} is empty.");
    }

    private sealed record SnipeItConfig(string BaseUrl, AccountsConfig Accounts, NewAsset NewAsset);

    private sealed record AccountsConfig(Account Admin, Account DemoUser);
}

public sealed record Account(string Username, string Password, string DisplayName);
