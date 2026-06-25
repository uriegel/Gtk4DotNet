using Gtk4DotNet;

Application
    .NewAdwaita("de.uriegel.Todo")
    .WithDiagnostics(true)
    .WithSettings()
    .OnActivate(app => app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    )
    .AccelsForAction("win.filter('All')", ["<Ctrl>A"])
    .AccelsForAction("win.filter('Open')", ["<Ctrl>O"])
    .AccelsForAction("win.filter('Done')", ["<Ctrl>D"])
    .Run();
