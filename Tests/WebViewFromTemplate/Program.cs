using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics()
    .OnActivate(app => app
        .WithWebKit()
        .WindowFromBuilder("template", "window", p => new MyWindow(p))
        .Show()
    ).Run();
