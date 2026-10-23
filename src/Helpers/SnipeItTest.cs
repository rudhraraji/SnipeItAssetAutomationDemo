using Microsoft.Playwright.NUnit;
using SnipeITDemo.Pages;

namespace SnipeITDemo.Helpers;

/// <summary>Base class for Snipe-IT tests, with shared steps such as logging in.</summary>
public abstract class SnipeItTest : PageTest
{
    /// <summary>Logs in and waits until the site has left the login page.</summary>
    protected async Task LogInAsync(Account account)
    {
        var loginPage = new LoginPage(Page);
        await loginPage.GoToAsync();
        await loginPage.LoginAsync(account.Username, account.Password);
        await Page.WaitForURLAsync(url => !url.Contains("/login", StringComparison.Ordinal));
    }
}
