using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        this.AddActions([
            new("quit", CloseWindow, "<Ctrl>Q"),
            new("preview", false, show => Console.WriteLine($"Preview: {show}"), "F3")
        ]);
    }
}

// TODO check IActionMap, GtkAction, ActionHandle: Action delegates are not being freed