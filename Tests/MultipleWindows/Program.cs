using CsTools.Extensions;
using Gtk4DotNet;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    using var window = app.NewWindow();
    window.Title = "Multiple Window👍";
    window.SetChild(new Button("Create Window")
        .SideEffect(b => b.OnClicked += () =>
        {
            using var win = new MyWindow(app);
            win.Show();
        }));
    window.Show();
};
app.Run();