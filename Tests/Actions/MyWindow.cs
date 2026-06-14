using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        AddActions(
            new BoolAction("preview", false, show => Console.WriteLine($"Preview: {show}"), "F3"),
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q")
        );
    }
}

// TODO one class for App, AppWin and ActionGroup
