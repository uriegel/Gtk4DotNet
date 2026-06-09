using System.Drawing;
using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics()
    .OnActivate(app => app
        .NewWindow()
        .Title("Hello WebView👍")
        .DefaultSize(800, 600)
        .Child(WebView
            .New()
            .BackgroundColor(Color.Transparent)
            .LoadUri("https://github.com/uriegel/Gtk4DotNet"))
        .Show()
    ).Run();


