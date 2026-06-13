using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        this.AddActions(
            new GtkAction("preview", false, show => Console.WriteLine($"Preview: {show}"), "F3"),
            new GtkAction("quit", CloseWindow, "<Ctrl>Q")
        );
    }
}

// TODO Actions for Window
// TODO SafeHandle, eliminate IActionMap