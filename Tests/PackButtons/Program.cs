using Gtk4DotNet;

using static System.Console;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    var window = app.NewWindow();
    window.Title = "Pack👍";
    using var button1 = new Button("Button 1");
    button1.OnClicked += () => WriteLine("Button1 clicked");
    using var button2 = new Button("Button 2");
    button2.OnClicked += () => WriteLine("Button2 clicked");
    using var button3 = new Button("Quit");
    button3.OnClicked += () => window.CloseWindow();
    window.SetChild(new Grid()
        .Attach(button1, 0, 0, 1, 1)
        .Attach(button2, 1, 0, 1, 1)
        .Attach(button3, 0, 1, 2, 1));
    window.Show();
};
app.Run();