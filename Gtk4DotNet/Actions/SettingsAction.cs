namespace Gtk4DotNet;

/// <summary>
/// An Action bound to a GSettings value
/// </summary>
/// <param name="Name">Action's name</param>
/// <param name="Action">The Gsettings action</param>
/// <param name="Accelerator">Gtk accelerator name</param>
public record SettingsAction(string Name, ActionHandle Action, string? Accelerator = null)
    : GtkAction(Name, Accelerator);
