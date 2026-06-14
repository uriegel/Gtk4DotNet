using Gtk4DotNet;

// TODO set description from file name
// TODO Cancel Button 
// TODO Shortcuts and actions in AdwDialog
// TODO default action "Open File"
// TODO AppChooserWidget in an AdwDialog
// TODO open file

class AppChooser : AdwDialog
{
    public AppChooser(Builder builder, string? name = null) : base(builder, name)
    {
        description.Text = "Das muss hier noch <b>ein wenig</b> abgeändert werden!!!!!!!!";

        using var actiongroup = SimpleActionGroup.New("appchooser");
        actiongroup.AddActions(
            new SimpleAction("openfile", () => Console.WriteLine("Öffne Datei")),
            new SimpleAction("test", () => Console.WriteLine("Test"))
        );
        InsertActionGroup("appchooser", actiongroup);

        AddShortcuts(
            Shortcut.New("appchooser.openfile", "<Ctrl>O"),
            Shortcut.New("appchooser.test", "<Ctrl>T")
        );
    }

    [Widget]
    readonly Label description = null!;
}