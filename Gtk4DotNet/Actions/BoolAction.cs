namespace Gtk4DotNet;

/// <summary>
/// A stateful action with a 'bool' state.
/// </summary>
/// <param name="Name">Action's name</param>
/// <param name="InitialState">The initial state</param>
/// <param name="StateChanged">Callback that is called when the state changes</param>
/// <param name="Accelerator">Gtk accelerator name</param>
public record BoolAction(string Name, bool InitialState, Action<bool> StateChanged, string? Accelerator = null)
    : GtkAction(Name, Accelerator);
