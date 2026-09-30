using System.Drawing;
using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.gtk4dotnet");
app.WithDiagnostics();
app.OnActivate += () =>
{
    using var window = app.NewWindow();
    window.Title = "Hello WebView👍";
    window.SetDefaultSize(800, 600);
    window.SetChild(WebView
        .New()
        .BackgroundColor(Color.Transparent)
        .LoadUri("https://github.com/uriegel/Gtk4DotNet"));
    window.Show();
};
app.Run();


