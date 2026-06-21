namespace Gtk4DotNet;

/// <summary>
/// A stateful action with a 'string' state.
/// </summary>
/// <param name="Name">Action's name</param>
/// <param name="InitialState">The initial state</param>
/// <param name="StateChanged">Callback that is called when the state changes</param>
/// <param name="Accelerator">Gtk accelerator name</param>
public record StringAction(string Name, string InitialState, Action<string> StateChanged, string? Accelerator = null)
    : GtkAction(Name, Accelerator);
