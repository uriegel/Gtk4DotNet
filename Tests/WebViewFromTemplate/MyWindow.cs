using System.Drawing;
using Gtk4DotNet;

class MyWindow : AdwApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        AddActions(
            new("quit", CloseWindow, "<Ctrl>Q"),
            new("devtools", async () =>
            {
                var inspector = webView.GetInspector();
                inspector.Show();
                await Task.Delay(400);
                inspector.Detach();
            }, "F12")
        );

        webView.BackgroundColor(Color.Transparent);
        var settings = webView.GetSettings();
        settings.EnableDeveloperExtras = true;
        webView.DisableContextMenu();
        webView.LoadUri("https://github.com/uriegel/Gtk4DotNet");
    }

    [Widget(Name="webview")]
    readonly WebView webView = null!;
}



