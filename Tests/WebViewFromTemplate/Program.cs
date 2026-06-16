using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .WithWebKit()
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    ).Run();
