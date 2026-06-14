namespace Gtk4DotNet;

public record BoolAction(string Name, bool InitialState, Action<bool> StateChanged, string? Accelerator = null)
    : GtkAction(Name, Accelerator);
