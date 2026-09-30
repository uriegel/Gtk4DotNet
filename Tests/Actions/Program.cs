using Gtk4DotNet;

var app = new AdwApplication("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    app.AddActions(new SimpleAction("test", () => Console.WriteLine("Test action from app"), "<Ctrl>T"));
    app.WindowFromBuilder("window", "window", p => new MyWindow(p))
        .Show();
};
app.Run();

