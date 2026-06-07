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

    [Widget]
    readonly ProgressDisplay revealer = null!;
}

