using Gtk4DotNet;

var app = new Application("de.uriegel.gtk4dotnet");
app.WithDiagnostics(true);
app.OnActivate += () =>
{
    using var window = app.NewWindow()
        .Title("Hello Picture👍")
        .DefaultSize(800, 800);

    using var pixbuf = new Pixbuf("/run/media/uwe/Daten/Bilder Rest/Tina/2021/10/20211007_172555.jpg");
    pixbuf.ApplyEmbeddedOrientation();

    var pic = Picture.New();
    pic.SetPixbuf(pixbuf);
    window.Child(pic);

    window.Show();
};
app.Run();


