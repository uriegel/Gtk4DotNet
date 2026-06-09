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

// TODO release Beta, 
// TODO WebWindowNetCore with WithAdwaita(), WithUIFromResource where the native WebView is contained
// TODO build WebWindowNetCore beta
// TODO GVolume, GDrive...
// TODO check Commander