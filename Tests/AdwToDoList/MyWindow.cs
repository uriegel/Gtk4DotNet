using CsTools.Extensions;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        entry.OnActivate(NewTask);

        using var shortcutsBuilder = Builder.FromDotNetResource("shortcuts");
        var shortcuts = new Window(shortcutsBuilder, "help_overlay");
        SetHelpOverlay(shortcuts);

        store = ListStore.New();
        store.Initialize(Persistence.Retrieve());

        settings = GSettings.New(Globals.ApplicationId);
        filterListModel = FilterListModel.New(store, GetFilter(settings));
        var model = NoSelection.New(filterListModel);

tasksList.Visible = true;        
        tasksList.BindModel<TaskItem>(model, "taskrow", CreateTaskRow);

        settings.OnChanged("filter", () => filterListModel.SetFilter(GetFilter(settings)));

        AddActions(
            new SimpleAction("remove-done-tasks", RemoveDoneTasks),
            settings.CreateAction("filter")
        );

        OnClose(_ => false.SideEffect(_ => Persistence.Save(store.GetItems<TaskItem>())));

        OnFinalize(() =>
        {
            model.Dispose();
            settings.Dispose();
        });
    }

    Widget CreateTaskRow(Builder builder, TaskItem? item)
    {
        var taskRow = new TaskRow(builder, "taskrow") ?? throw new Exception("TaskRow is null");
        if (item != null)
            taskRow.SetTask(item);
        return taskRow;
    }

    void NewTask()
    {
        var editable = entry.AsEditable();
        var text = editable.Text;
        if (text == "")
            return;
        editable.Text = "";
        store.Append(new TaskItem(false, text));
    }

    void RemoveDoneTasks()
    {
        var donePositions = store
            .GetItems<TaskItem>()
            .Select((n, i) => (Task: n, Pos: i))
            .Where(n => n.Task.Completed)
            .Select(n => n.Pos)
            .Reverse()
            .ToArray();
        foreach (var pos in donePositions)
            store.Remove(pos);
    }

    static CustomFilter? GetFilter(GSettings settings)
        => settings.GetString("filter") switch
        {
            "Open" => CustomFilter.New<TaskItem>(item => item?.Completed != true),
            "Done" => CustomFilter.New<TaskItem>(item => item?.Completed == true),
            _ => null
        };

    [Widget]
    readonly ListBox tasksList = null!;

    [Widget]
    readonly Entry entry = null!;

    readonly ListStore store;

    readonly GSettings settings;

    FilterListModel filterListModel = null!;
}

record TaskItem(string Content)
{
    public TaskItem() : this("") {}
    public TaskItem(bool completed, string content) : this(content) => Completed = completed;
    public bool Completed { get; set; }
}