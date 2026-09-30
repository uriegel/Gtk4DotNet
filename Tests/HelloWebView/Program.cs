using System.Drawing;
using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.gtk4dotnet");
app.WithDiagnostics();
app.OnActivate += () =>
    app.NewWindow()
        .Title("Hello WebView👍")
        .DefaultSize(800, 600)
        .Child(WebView
            .New()
            .BackgroundColor(Color.Transparent)
            .LoadUri("https://github.com/uriegel/Gtk4DotNet"))
        .Show();
app.Run();


