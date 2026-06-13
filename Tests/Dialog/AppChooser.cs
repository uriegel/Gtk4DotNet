using Gtk4DotNet;

class AppChooser : AdwDialog
{
    public AppChooser(Builder builder, string? name = null) : base(builder, name)
    {
        description.Text = "Das muss hier noch <b>ein wenig</b> abgeändert werden!!!!!!!!";

        // this.AddActions("appchooser", 
        //     new GtkAction("openfile", () => Console.WriteLine("Öffne Datei")));
    }

    [Widget]
    readonly Label description = null!;
}