using Gtk4DotNet;

class TaskRow : Box
{
    public TaskRow(Builder builder, string name) : base(builder, name) { }

    public void SetTask(TaskItem task)
    {
        this.task = task;
        completedButton.IsActive = task.Completed;
        contentLabel.Text = task.Content;
        completedButton.OnToggled -= OnToggleCompleted;
        completedButton.OnToggled += OnToggleCompleted;
    }

    void OnToggleCompleted(bool state) => task?.Completed = state;

    [Widget]
    readonly CheckButton completedButton = null!;

    [Widget]
    readonly Label contentLabel = null!;

    TaskItem? task;
}