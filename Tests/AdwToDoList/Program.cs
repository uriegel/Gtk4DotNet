using Gtk4DotNet;

Application
    .NewAdwaita(Globals.ApplicationId)
    .WithSettings()
    .WithDiagnostics(true)
    .OnActivate(app => app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    )
    .AccelsForAction("win.filter('All')", ["<Ctrl>A"])
    .AccelsForAction("win.filter('Open')", ["<Ctrl>O"])
    .AccelsForAction("win.filter('Done')", ["<Ctrl>D"])
    .Run();
