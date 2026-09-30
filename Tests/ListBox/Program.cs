using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.gtk4dotnet");
app.WithDiagnostics();
app.OnActivate += () =>
    app.WindowFromBuilder("template", "window", p => new MyWindow(p))
    .Show();
app.Run();


