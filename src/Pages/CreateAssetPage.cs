using Microsoft.Playwright;
using SnipeITDemo.Helpers;

namespace SnipeITDemo.Pages;

public sealed class CreateAssetPage
{
    private readonly IPage _page;
    private readonly ILocator _assetTagInput;
    private readonly ILocator _modelDropdown;
    private readonly ILocator _statusDropdown;
    private readonly ILocator _checkoutToUserRadio;
    private readonly ILocator _userDropdown;
    private readonly ILocator _redirectOption;
    private readonly ILocator _saveButton;
    private readonly ILocator _openDropdownSearch;
    private readonly ILocator _openDropdownOptions;

    public CreateAssetPage(IPage page)
    {
        _page = page;
        _assetTagInput = page.Locator("#asset_tag");
        _modelDropdown = page.Locator("#select2-model_select_id-container");
        _statusDropdown = page.Locator("#select2-status_select_id-container");
        _checkoutToUserRadio = page.Locator("input[name='checkout_to_type'][value='user']");
        _userDropdown = page.Locator("#select2-assigned_user_select-container");
        _redirectOption = page.Locator("select[name='redirect_option']");
        _saveButton = page.Locator("#submit_button");
        _openDropdownSearch = page.Locator(".select2-container--open .select2-search__field");
        _openDropdownOptions = page.Locator(
            ".select2-container--open .select2-results__option[role='option']:not(.loading-results)");
    }

    public static string Url => SnipeItSettings.Url("/hardware/create");

     public async Task<CreatedAsset> CreateAssetAsync(NewAsset asset)
    {
        await GoToAsync();
        var tag = await GetAssetTagAsync();
        await SelectModelAsync(asset.Model);
        await SelectStatusAsync(asset.Status);
        var userName = await CheckoutToRandomUserAsync();
        await SaveAndViewAssetAsync();
        return new CreatedAsset(tag, asset.Model, asset.Status, userName, _page.Url);
    }

    public async Task GoToAsync()
    {
        await _page.GotoAsync(Url);
    }

    /// <summary>The asset tag the site pre-fills for a new asset.</summary>
    public async Task<string> GetAssetTagAsync()
    {
        return await _assetTagInput.InputValueAsync();
    }

    public async Task SelectModelAsync(string modelName)
    {
        await _modelDropdown.ClickAsync();
        await _openDropdownSearch.FillAsync(modelName);
        await _openDropdownOptions.Filter(new() { HasText = modelName }).First.ClickAsync();
    }

    public async Task SelectStatusAsync(string statusName)
    {
        await _statusDropdown.ClickAsync();
        await _openDropdownOptions.GetByText(statusName, new() { Exact = true }).ClickAsync();
    }

    /// <summary>Checks the asset out to a randomly chosen user and returns that user's name.</summary>
    public async Task<string> CheckoutToRandomUserAsync()
    {
        await _checkoutToUserRadio.CheckAsync(new() { Force = true });
        await _userDropdown.ClickAsync();
        await _openDropdownOptions.First.WaitForAsync();

        var userCount = await _openDropdownOptions.CountAsync();
        var user = _openDropdownOptions.Nth(Random.Shared.Next(userCount));
        var optionText = (await user.InnerTextAsync()).Trim();
        await user.ClickAsync();

        // Options read "First Last (username) - #employee"; the rest of the site shows just "First Last".
        return optionText.Split(" (")[0].Trim();
    }

    public async Task<AssetDetailsPage> SaveAndViewAssetAsync()
    {
        await _redirectOption.SelectOptionAsync("item", new() { Force = true });
        await _saveButton.ClickAsync();
        return await AssetDetailsPage.WaitForAsync(_page);
    }
}
