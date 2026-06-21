namespace Gtk4DotNet;

/// <summary>
/// A simple Gtk Action
/// </summary>
/// <param name="Name">Action's name</param>
/// <param name="Action">Callback that fires when the action is triggered</param>
/// <param name="Accelerator">Gtk accelerator name</param>
public record SimpleAction(string Name, Action Action, string? Accelerator = null)
    : GtkAction(Name, Accelerator);
