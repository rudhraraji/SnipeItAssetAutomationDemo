using Microsoft.Playwright;
using SnipeITDemo.Helpers;

namespace SnipeITDemo.Pages;

public sealed class AssetsListPage
{
    private readonly IPage _page;
    private readonly ILocator _searchInput;
    private readonly ILocator _rows;

    public AssetsListPage(IPage page)
    {
        _page = page;
        _searchInput = page.Locator(".bootstrap-table .search input");
        _rows = page.Locator("#assetsListingTable tbody tr");
    }

    public static string Url => SnipeItSettings.Url("/hardware");

    public async Task GoToAsync()
    {
        await _page.GotoAsync(Url, new() { WaitUntil = WaitUntilState.DOMContentLoaded });
    }

    public async Task<ILocator> FindAssetAsync(string assetTag)
    {
        await GoToAsync();
        return await SearchAsync(assetTag);
    }

    public async Task<ILocator> SearchAsync(string term)
    {
        await _searchInput.FillAsync(term);
        await _searchInput.PressAsync("Enter");
        return _rows.Filter(new() { HasText = term });
    }

    public static ILocator AssetLink(ILocator row, string assetTag)
    {
        return row.GetByRole(AriaRole.Link, new() { Name = assetTag, Exact = true });
    }

    public async Task<AssetDetailsPage> OpenAssetAsync(ILocator row, string assetTag)
    {
        await AssetLink(row, assetTag).ClickAsync();
        return await AssetDetailsPage.WaitForAsync(_page);
    }
}
