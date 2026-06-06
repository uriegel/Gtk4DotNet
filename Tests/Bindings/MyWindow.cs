using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
    }

    [Widget(Name = "button1")]
    readonly Button button1 = null!;
    [Widget(Name = "button2")]
    readonly Button button2 = null!;
    [Widget(Name = "quit")]
    readonly Button quit = null!;
}