using Gtk4DotNet;

class AppChooser : AdwDialog
{
    public AppChooser(Builder builder, string? name = null) : base(builder, name)
    {
        description.Text = "Choose an Application to open &lt;b&gt;this file&lt;/b&gt;";
        SetDefaultWidget(openBtn);

        using var actiongroup = SimpleActionGroup.New("appchooser");
        actiongroup.AddActions(
            new SimpleAction("openfile", () => Console.WriteLine("Open File")),
            new SimpleAction("cancel", CloseDialog)
        );
        InsertActionGroup("appchooser", actiongroup);

        AddShortcuts(
            Shortcut.New("appchooser.openfile", "<Ctrl>O"),
            Shortcut.New("appchooser.cancel", "<Cancel>")
        );
    }

    [Widget]
    readonly Button openBtn = null!;

    [Widget]
    readonly Label description = null!;
}