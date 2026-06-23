using CsTools.HttpRequest;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        entry.OnActivate(NewTask);

        store = ListStore.New();
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

        settings = GSettings.New(Globals.ApplicationId);
        filterListModel = FilterListModel.New(store, GetFilter(settings));
        var model = SingleSelection.New(filterListModel);
        tasksList.SetModel(model);
        tasksList.SetFactory(factory);

        settings.OnChanged("filter", () => filterListModel.SetFilter(GetFilter(settings)));

        AddActions(
            new SimpleAction("remove-done-tasks", RemoveDoneTasks),
            settings.CreateAction("filter")
        );

        OnFinalize(() =>
        {
            factory.Dispose();
            model.Dispose();
            settings.Dispose();
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
            .Reverse()
            .ToArray();
        foreach (var pos in donePositions)
            store.Remove(pos);
    }

    CustomFilter? GetFilter(GSettings settings)
        => settings.GetString("filter") switch
        {
            "Open" => CustomFilter.New<TaskItem>(item => item?.Completed != true),
            "Done" => CustomFilter.New<TaskItem>(item => item?.Completed == true),
            _ => null
        };


    [Widget]
    readonly ListView tasksList = null!;

    [Widget]
    readonly Entry entry = null!;

    readonly ListStore store;

    readonly GSettings settings;

    FilterListModel filterListModel = null!;
}

record TaskItem(string Content)
{
    public TaskItem(bool completed, string content) : this(content) => Completed = completed;
    public bool Completed { get; set; }
}