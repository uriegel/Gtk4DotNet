using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        button1.OnClicked(() => Console.WriteLine("Button1 clicked"));
        button2.OnClicked(() => Console.WriteLine("Button2 clicked"));
        quit.OnClicked(CloseWindow);
    }

    [Widget(Name = "button1")]
    readonly Button button1 = null!;
    [Widget(Name = "button2")]
    readonly Button button2 = null!;
    [Widget(Name = "quit")]
    readonly Button quit = null!;
}