namespace Gtk4DotNet;

public record SimpleAction(string Name, Action Action, string? Accelerator = null) 
    : GtkAction(Name, Accelerator);
