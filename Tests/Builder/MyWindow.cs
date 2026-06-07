using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        button1.OnClicked(() => Console.WriteLine("Button1 clicked"));
        button2.OnClicked(() => Console.WriteLine("Button2 clicked"));
        quit.OnClicked(CloseWindow);
    }

    [Widget]
    readonly Button button1 = null!;

    [Widget]
    readonly Button button2 = null!;
    
    [Widget]
    readonly Button quit = null!;
}