using Microsoft.Playwright;

namespace SnipeITDemo.Pages;

public sealed class AccountPage
{
    private readonly IPage _page;

    public AccountPage(IPage page)
    {
        _page = page;
    }

    /// show the name in the menu
    /// Return false if it never appears.
    public async Task<bool> IsLoadedAsync(string userDisplayName)
    {
        try
        {
            await _page.WaitForURLAsync("**/account/view-assets", new() { WaitUntil = WaitUntilState.DOMContentLoaded });
            await _page.GetByRole(AriaRole.Link, new() { Name = userDisplayName }).WaitForAsync();
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }
}
