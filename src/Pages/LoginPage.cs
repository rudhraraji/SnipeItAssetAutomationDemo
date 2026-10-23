using Microsoft.Playwright;
using SnipeITDemo.Helpers;

namespace SnipeITDemo.Pages;

public sealed class LoginPage
{
    private readonly IPage _page;
    private readonly ILocator _usernameInput;
    private readonly ILocator _passwordInput;
    private readonly ILocator _rememberMeCheckbox;
    private readonly ILocator _loginButton;

    public LoginPage(IPage page)
    {
        _page = page;
        _usernameInput = page.GetByRole(AriaRole.Textbox, new() { Name = "Username" });
        _passwordInput = page.GetByRole(AriaRole.Textbox, new() { Name = "Password" });
        _rememberMeCheckbox = page.GetByRole(AriaRole.Checkbox, new() { Name = "Remember Me" });
        _loginButton = page.GetByRole(AriaRole.Button, new() { Name = "Login" });
    }

    public static string Url => SnipeItSettings.Url("/login");

    public async Task GoToAsync()
    {
        await _page.GotoAsync(Url);
    }

    public async Task<bool> IsLoadedAsync()
    {
        return await _page.GetByRole(AriaRole.Heading, new() { Name = "Please Login" }).IsVisibleAsync()
            && await _usernameInput.IsVisibleAsync()
            && await _passwordInput.IsVisibleAsync()
            && await _loginButton.IsVisibleAsync();
    }

    public async Task<AccountPage> LoginAsync(string username, string password, bool rememberMe = false)
    {
        await _usernameInput.FillAsync(username);
        await _passwordInput.FillAsync(password);

        if (rememberMe)
        {
            await _rememberMeCheckbox.CheckAsync();
        }

        await _loginButton.ClickAsync();
        return new AccountPage(_page);
    }
}
