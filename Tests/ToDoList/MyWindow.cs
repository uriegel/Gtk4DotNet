using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        entry.OnActivate(NewTask);

        store = ListStore.New();
        var model = SingleSelection.New(store);
        var factory = SignalListItemFactory.New();
        factory.Setup(listitem =>
        {
            using var builder = Builder.FromDotNetResource("taskrow");
            var taskRow = new TaskRow(builder, "taskrow") ?? throw new Exception("TaskRow is null");
            listitem.SetManagedChild(taskRow);
        });
        factory.Bind(listitem =>
        {
            var taskRow = listitem.GetManagedChild<TaskRow>();
            var item = listitem.GetItem<TaskItem>();
            if (item != null)
                taskRow?.SetTask(item);
        });

        tasksList.SetModel(model);
        tasksList.SetFactory(factory);

        AddActions(
            new SimpleAction("remove-done-tasks", RemoveDoneTasks)
        );

        OnFinalize(() =>
        {
            factory.Dispose();
            model.Dispose();
        });
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
            .ToArray();
    }

    [Widget]
    readonly ListView tasksList = null!;

    [Widget]
    readonly Entry entry = null!;

    readonly ListStore store;
}

record TaskItem(string Content)
{
    public TaskItem(bool completed, string content) : this(content) => Completed = completed;
    public bool Completed { get; set; }
}