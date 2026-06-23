using Gtk4DotNet;

class TaskRow : Box
{
    public TaskRow(Builder builder, string name) : base(builder, name)
    {
    }

    [Widget]
    readonly CheckButton completedButton = null!;

    [Widget]
    readonly Label contentLabel = null!;
}