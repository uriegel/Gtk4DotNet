namespace Gtk4DotNet;

public class ApplicationWindow : Window // , IActionMap
{
    public ApplicationWindow(WindowBuilder builder) : base(builder.Builder, builder.Window)
        => SetApplication(this, builder.Application);
}


