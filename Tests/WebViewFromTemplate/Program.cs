using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.WithWebKit();
app.OnActivate += () =>
    app.WindowFromBuilder("window", "window", p => new MyWindow(p))
    .Show();
app.Run();
