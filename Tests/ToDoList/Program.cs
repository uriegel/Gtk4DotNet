using Gtk4DotNet;

Application
    .New("de.uriegel.Todo")
    .WithDiagnostics(true)
    .WithSettings()
    .OnActivate(app => app
        .WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show()
    )
    .AccelsForAction("win.filter('All')", ["<Ctrl>A"])
    .AccelsForAction("win.filter('Open')", ["<Ctrl>O"])
    .AccelsForAction("win.filter('Done')", ["<Ctrl>D"])
    .AccelsForAction("win.show-help-overlay", ["<Ctrl>H"])
    .Run();
