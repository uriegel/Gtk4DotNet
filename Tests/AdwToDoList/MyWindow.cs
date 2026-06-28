using CsTools.Extensions;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        entry.OnActivate += NewTask;

        store = ListStore.New();
        store.OnItemsChanged += (p, r, a) => tasksList.Visible = store.GetItems() > 0;

        filterListModel = FilterListModel.New(store, GetFilter(Application.Settings));
        var model = NoSelection.New(filterListModel);

        tasksList.BindModel<TaskItem>(model, "taskrow", CreateTaskRow);

        Application.Settings["filter"].OnChanged += OnFilterChanged;
        store.Initialize(Persistence.Retrieve());

        AddActions(
            new SimpleAction("remove-done-tasks", RemoveDoneTasks),
            new SimpleAction("show-help-overlay", ShowHelp, "<Ctrl>H"),
            Application.Settings.CreateAction("filter")
        );

        OnClose(_ => false.SideEffect(_ => Persistence.Save(store.GetItems<TaskItem>())));

        OnFinalize(() =>
        {
            model.Dispose();
            Application.Settings["filter"].OnChanged -= OnFilterChanged;
        });
    }

    void OnFilterChanged() => filterListModel.SetFilter(GetFilter(Application.Settings));

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

    void ShowHelp()
    {
        using var shortcutsBuilder = Builder.FromDotNetResource("shortcuts");
        var shortcuts = new AdwDialog(shortcutsBuilder, "help_overlay");
        shortcuts.Present(this);
    }

    [Widget]
    readonly ListBox tasksList = null!;

    [Widget]
    readonly Entry entry = null!;

    readonly ListStore store;

    FilterListModel filterListModel = null!;
}

record TaskItem(string Content)
{
    public TaskItem() : this("") {}
    public TaskItem(bool completed, string content) : this(content) => Completed = completed;
    public bool Completed { get; set; }
}