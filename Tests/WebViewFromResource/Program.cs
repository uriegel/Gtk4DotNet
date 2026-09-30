using System.Drawing;
using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.gtk4dotnet")
{
    WebsiteFromResource = true
};
app.WithDiagnostics();
app.OnActivate += () =>
    app.NewWindow()
    .Title("WebView from Resource👍")
    .DefaultSize(800, 600)
    .Child(WebView
        .New()
        .BackgroundColor(Color.Transparent)
        .LoadUri("res://website/index.html"))
    .Show();
app.Run();


