using System.Drawing;
using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        AddActions(
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q"),
            new SimpleAction("devtools", webView.ShowInspector, "F12")
        );

        webView.BackgroundColor(Color.Transparent);
        var settings = webView.GetSettings();
        settings.EnableDeveloperExtras = true;
        webView.DisableContextMenu();
        webView.OnAlert((_,_) => Console.WriteLine("Tst"));
        webView.LoadUri("https://github.com/uriegel/Gtk4DotNet");
    }

    [Widget(Name="webview")]
    readonly WebView webView = null!;
}



