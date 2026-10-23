namespace SnipeITDemo.Helpers;

/// To Create Asset form.
public sealed record NewAsset(string Model, string Status);

/// Create Asset 
public sealed record CreatedAsset(string Tag, string Model, string Status, string AssignedUser, string Url);

/// Aasset's History tab.
public sealed record HistoryEntry(string Action, string CreatedBy, string Item, string Target, string Note);
