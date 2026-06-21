using System.Diagnostics.CodeAnalysis;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        using var display = Display.GetDefault();
        using var cssProvider = CssProvider.New().FromResource("style");
        StyleContext.AddProviderForDisplay(
            display,
            cssProvider,
            StyleProviderPriority.Application);
    }

    // TODO to REAMDME.md
    [SuppressMessage("Compiler", "CS0414", Justification = "Bound from GtkBuilder template")]
    [Widget]
    readonly ProgressDisplay revealer = null!;
}

