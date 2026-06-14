using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .WindowFromBuilder("template", "window", p => new MyWindow(p))
        .Show()
    ).Run();


