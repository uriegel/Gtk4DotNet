using Gtk4DotNet;

Application
    .NewAdwaita(Globals.ApplicationId)
    .WithDiagnostics(true)
    .OnActivate(app => app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    ).Run();
