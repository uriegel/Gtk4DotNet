namespace Gtk4DotNet;

public record StringAction(string Name, string InitialState, Action<string> StateChanged, string? Accelerator = null)
    : GtkAction(Name, Accelerator);
