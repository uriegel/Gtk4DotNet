using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        AddActions(
            new("preview", false, show => Console.WriteLine($"Preview: {show}"), "F3"),
            new("quit", CloseWindow, "<Ctrl>Q")
        );
    }
}

// TODO 4 dangling Action delegates
// TODO ActionHandle => SimpleAction
// TODO one class for App, AppWin and ActionGroup
