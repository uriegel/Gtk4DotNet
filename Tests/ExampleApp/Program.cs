using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.exampleapp", ApplicationFlags.HandlesOpen);
app.WithDiagnostics(true);
app.WithSettings();
app.OnOpen += files =>
{
    if (MyWindow.Instance == null)
        app.WindowFromBuilder("window", "window", p => new MyWindow(p))
            .Show();
    foreach (var file in files)
        MyWindow.Instance?.OnOpen(file);
};
app.OnActivate += () =>
    app.WindowFromBuilder("window", "window", p => new MyWindow(p))
    .Show();
app.Run();
