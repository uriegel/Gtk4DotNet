using CsTools.Extensions;
using Gtk4DotNet;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
    app.NewWindow()
    .Title("Multiple Window👍")
    .Child(Button
        .NewWithLabel("Create Window")
        .SideEffect(b => b.OnClicked += () =>
        {
            var win = new MyWindow(app);
            win.Show();
        }))
    .Show();
app.Run();