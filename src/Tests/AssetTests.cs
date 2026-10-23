using Microsoft.Playwright;
using SnipeITDemo.Pages;
using SnipeITDemo.Helpers;

namespace SnipeITDemo.Tests;

public class AssetTests : SnipeItTest
{
    [SetUp]
    public async Task LogInAsAdmin()
    {
        await LogInAsync(SnipeItSettings.Admin);
    }

    [Test]
    public async Task CreateReadyToDeployMacbookCheckedOutToRandomUser()
    {
        var asset = await new CreateAssetPage(Page).CreateCheckedOutToRandomUserAsync(SnipeItSettings.NewAsset);

        var assetsListPage = new AssetsListPage(Page);
        var assetRow = await assetsListPage.FindAssetAsync(asset.Tag);
        await ExpectListRowMatchesAsync(assetRow, asset);

        var detailsPage = await assetsListPage.OpenAssetAsync(assetRow, asset.Tag);
        await ExpectDetailsMatchAsync(detailsPage, asset);
        await ExpectCreatedAndCheckedOutHistoryAsync(detailsPage, asset);
    }

    private async Task ExpectListRowMatchesAsync(ILocator row, CreatedAsset asset)
    {
        await Expect(row).ToHaveCountAsync(1);
        await Expect(row).ToContainTextAsync(asset.Model);
        await Expect(row).ToContainTextAsync(asset.AssignedUser);
    }

    private async Task ExpectDetailsMatchAsync(AssetDetailsPage detailsPage, CreatedAsset asset)
    {
        Assert.That(Page.Url, Is.EqualTo(asset.Url), "The list should link to the asset that was just created.");
        await Expect(detailsPage.Heading).ToContainTextAsync(asset.Tag);
        await Expect(detailsPage.Heading).ToContainTextAsync(asset.Model);
        await Expect(detailsPage.Content).ToContainTextAsync(asset.Status);
        await Expect(detailsPage.Content).ToContainTextAsync("Deployed");
        await Expect(detailsPage.AssignedUserLink(asset.AssignedUser)).ToBeVisibleAsync();
    }

    private async Task ExpectCreatedAndCheckedOutHistoryAsync(AssetDetailsPage detailsPage, CreatedAsset asset)
    {
        await detailsPage.OpenHistoryTabAsync();
        await Expect(detailsPage.HistoryRows).ToHaveCountAsync(2);

        var history = await detailsPage.GetHistoryAsync();
        var created = history.SingleOrDefault(e => e.Action == "create new");
        var checkedOut = history.SingleOrDefault(e => e.Action == "checkout");
        var item = $"#{asset.Tag} - {asset.Model}";
        var admin = SnipeItSettings.Admin.DisplayName;

        Assert.That(created, Is.Not.Null, "History should have one 'create new' entry.");
        Assert.That(checkedOut, Is.Not.Null, "History should have one 'checkout' entry.");
        Assert.Multiple(() =>
        {
            Assert.That(created!.CreatedBy, Is.EqualTo(admin));
            Assert.That(created.Item, Does.Contain(item));

            Assert.That(checkedOut!.CreatedBy, Is.EqualTo(admin));
            Assert.That(checkedOut.Item, Does.Contain(item));
            Assert.That(checkedOut.Target, Is.EqualTo(asset.AssignedUser));
            Assert.That(checkedOut.Note, Is.EqualTo("Checked out on asset creation"));
        });
    }
}
