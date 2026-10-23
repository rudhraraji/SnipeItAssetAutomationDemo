using SnipeITDemo.Pages;
using SnipeITDemo.Helpers;

namespace SnipeITDemo.Tests;

public class LoginTests : SnipeItTest
{
    [Test]
    public async Task LoginPageDisplaysLoginForm()
    {
        var loginPage = new LoginPage(Page);
        await loginPage.GoToAsync();

        Assert.That(await loginPage.IsLoadedAsync(), Is.True);
    }

    [Test]
    public async Task DemoUserCanLogIn()
    {
        var user = SnipeItSettings.DemoUser;
        var loginPage = new LoginPage(Page);
        await loginPage.GoToAsync();
        var accountPage = await loginPage.LoginAsync(user.Username, user.Password);

        Assert.That(await accountPage.IsLoadedAsync(user.DisplayName), Is.True);
    }
}
