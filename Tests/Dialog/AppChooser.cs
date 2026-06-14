using Gtk4DotNet;

class AppChooser : AdwDialog
{
    public AppChooser(Builder builder, string? name = null) : base(builder, name)
    {
        description.Text = "Das muss hier noch <b>ein wenig</b> abgeändert werden!!!!!!!!";

        var actiongroup = SimpleActionGroup.New();
        actiongroup.AddActions(new SimpleAction("openfile", () => Console.WriteLine("Öffne Datei")));
        InsertActionGroup("appchooser", actiongroup);
    }

    [Widget]
    readonly Label description = null!;
}