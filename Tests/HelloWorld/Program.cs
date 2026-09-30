using Gtk4DotNet;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    using var window = app.NewWindow();
    window.Title = "Hello World👍";
    window.SetDefaultSize(600, 200);
    window.Show();
};
app.Run();


