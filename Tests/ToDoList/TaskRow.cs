using Gtk4DotNet;

class TaskRow : Box
{
    public TaskRow(Builder builder, string name) : base(builder, name) { }

    public void SetTask(TaskItem task)
    {
        completedButton.IsActive = task.Completed;
        contentLabel.Text = task.Content;
    }

    [Widget]
    readonly CheckButton completedButton = null!;

    [Widget]
    readonly Label contentLabel = null!;
}