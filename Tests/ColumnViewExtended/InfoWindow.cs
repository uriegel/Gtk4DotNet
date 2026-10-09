using Gtk4DotNet;

class InfoWindow : Window
{
    public InfoWindow(Application app, Builder builder) : base(builder, "window")
    {
        app.AddWindow(this);

        var infos = Gtk.GtkInfo();
        label.Text = string.Join("\r\n", infos);
    }

    [Widget]
    readonly Label label = null!;
}
