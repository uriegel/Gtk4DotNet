using System.Drawing;
using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        this.AddActions([
            new("quit", CloseWindow, "<Ctrl>Q"),
            new("preview", false, show => Console.WriteLine($"Preview: {show}"), "F3")
        ]);

        webView.BackgroundColor(Color.Transparent);
        var settings = webView.GetSettings();
        settings.EnableDeveloperExtras = true;
        webView.LoadUri("https://github.com/uriegel/Gtk4DotNet");
    }

    [Widget(Name="webview")]
    readonly WebView webView = null!;
}

// TODO Action for opening Inspector and detach it
// TODO Disable Context Menu
