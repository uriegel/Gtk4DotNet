using Gtk4DotNet;

// TODO Implement Actions in Action group
// TODO Implement Shortcuts
// TODO set description from file name
// TODO Cancel Button 
// TODO default action "Open File"
// TODO AppChooserWidget in an AdwDialog
// TODO open file

class AppChooser : AdwDialog
{
    public AppChooser(Builder builder, string? name = null) : base(builder, name)
    {
        description.Text = "Das muss hier noch <b>ein wenig</b> abgeändert werden!!!!!!!!";

        using var actiongroup = SimpleActionGroup.New();
        actiongroup.AddActions(new SimpleAction("openfile", () => Console.WriteLine("Öffne Datei")));
        InsertActionGroup("appchooser", actiongroup);
    }

    [Widget]
    readonly Label description = null!;
}