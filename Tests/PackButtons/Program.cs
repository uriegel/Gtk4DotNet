using Gtk4DotNet;
using CsTools.Extensions;

using static System.Console;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
    app.NewWindow()
    .Title("Pack👍")
    .Pipe(win => win.Child(
        Grid
            .New()
            .Attach(
                Button
                    .NewWithLabel("Button 1")
                    .SideEffect(b => b.OnClicked += () => WriteLine("Button1 clicked")), 0, 0, 1, 1)
            .Attach(
                Button
                    .NewWithLabel("Button 2")
                    .SideEffect(b => b.OnClicked += () => WriteLine("Button2 clicked")), 1, 0, 1, 1)
            .Attach(
                Button
                    .NewWithLabel("Quit")
                    .SideEffect(b => b.OnClicked += () => win.CloseWindow()), 0, 1, 2, 1)))
    .Show();
app.Run();