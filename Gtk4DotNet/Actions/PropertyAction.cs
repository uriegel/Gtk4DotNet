namespace Gtk4DotNet;

public record PropertyAction(string Name, ActionHandle Action, string? Accelerator = null)
    : GtkAction(Name, Accelerator);

