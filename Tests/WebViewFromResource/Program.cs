using System.Drawing;
using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.gtk4dotnet")
{
    WebsiteFromResource = true
};
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    using var window = app.NewWindow();
    window.Title = "WebView from Resource👍";
    window.SetDefaultSize(800, 600);
    window.SetChild(new WebView()
        .BackgroundColor(Color.Transparent)
        .LoadUri("res://website/index.html"));
    window.Show();
};
app.Run();


