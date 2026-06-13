using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        AddActions(
            new GtkAction("preview", false, show => Console.WriteLine($"Preview: {show}"), "F3"),
            new GtkAction("quit", CloseWindow, "<Ctrl>Q")
        );
    }
}

// TODO ActionHandle => SimpleAction
// TODO one class for App, AppWin and ActionGroup
