using Gtk4DotNet;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    using var window = app.NewWindow();
    window.Title = "Hello Picture👍";
    window.SetDefaultSize(800, 800);
    using var pixbuf = new Pixbuf("pic.jpg");
    pixbuf.ApplyEmbeddedOrientation();
    using var pic = new Picture(pixbuf);
    window.SetChild(pic);
    window.Show();
};
app.Run();


