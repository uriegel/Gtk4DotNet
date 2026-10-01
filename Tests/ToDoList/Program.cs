using Gtk4DotNet;

var app = new Application("de.uriegel.Todo");
app.WithDiagnostics(true);
app.WithSettings();
app.OnActivate += () =>
    app.WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show();
app.SetAccelsForAction("win.filter('All')", ["<Ctrl>A"]);
app.SetAccelsForAction("win.filter('Open')", ["<Ctrl>O"]);
app.SetAccelsForAction("win.filter('Done')", ["<Ctrl>D"]);
app.SetAccelsForAction("win.show-help-overlay", ["<Ctrl>H"]);
app.Run();
