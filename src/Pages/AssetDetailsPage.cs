using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using SnipeITDemo.Helpers;

namespace SnipeITDemo.Pages;

public sealed partial class AssetDetailsPage
{
    private readonly ILocator _historyTab;
    private readonly ILocator _historyTable;

    public AssetDetailsPage(IPage page)
    {
        Heading = page.GetByRole(AriaRole.Heading, new() { Level = 1 });
        Content = page.Locator("#main");
        _historyTab = page.Locator("a[href='#history']");
        _historyTable = page.Locator("#assetHistoryListingTable");
        HistoryRows = _historyTable.Locator("tbody tr");
    }

    public ILocator Heading { get; }

    public ILocator Content { get; }

    public ILocator HistoryRows { get; }

    /// <summary>Waits until the browser is on an asset's details page.</summary>
    public static async Task<AssetDetailsPage> WaitForAsync(IPage page)
    {
        // The demo site's full "load" event can be very slow; the DOM is all the details page needs.
        await page.WaitForURLAsync(AssetUrlPattern(), new() { WaitUntil = WaitUntilState.DOMContentLoaded });
        return new AssetDetailsPage(page);
    }

    public ILocator AssignedUserLink(string userName)
    {
        return Content.GetByRole(AriaRole.Link, new() { Name = userName, Exact = true });
    }

    public async Task OpenHistoryTabAsync()
    {
        await _historyTab.ClickAsync();
        // The history table loads its rows over AJAX after the tab opens.
        await HistoryRows.First.WaitForAsync();
    }

    /// <summary>Reads every row currently shown in the History tab.</summary>
    public async Task<IReadOnlyList<HistoryEntry>> GetHistoryAsync()
    {
        // Map each cell to its column's data-field so the result doesn't depend on column order.
        var json = await _historyTable.EvaluateAsync<string>(
            """
            table => {
                const fields = [...table.querySelectorAll('thead th')].map(th => th.dataset.field);
                return JSON.stringify([...table.querySelectorAll('tbody tr')].map(tr =>
                    Object.fromEntries([...tr.children].map((td, i) => [fields[i] ?? `col${i}`, td.innerText.trim()]))));
            }
            """);
        var rows = JsonSerializer.Deserialize<Dictionary<string, string>[]>(json) ?? [];

        return [.. rows.Select(row => new HistoryEntry(
            Action: row.GetValueOrDefault("action_type", ""),
            CreatedBy: row.GetValueOrDefault("created_by", ""),
            Item: row.GetValueOrDefault("item", ""),
            Target: row.GetValueOrDefault("target", ""),
            Note: row.GetValueOrDefault("note", "")))];
    }

    [GeneratedRegex(@"/hardware/\d+$")]
    private static partial Regex AssetUrlPattern();
}
