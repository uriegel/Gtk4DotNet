using System.Drawing;
using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .WithWebsiteFromResource()
    .OnActivate(app => app
        .NewWindow()
        .Title("WebView from Resource👍")
        .DefaultSize(800, 600)
        .Child(WebView
            .New()
            .BackgroundColor(Color.Transparent)
            .LoadUri("res://website/index.html"))
        .Show()
    ).Run();


