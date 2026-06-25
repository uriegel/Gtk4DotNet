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

        filterListModel = FilterListModel.New(store, GetFilter(Application.Settings));
        var model = SingleSelection.New(filterListModel);
        tasksList.SetModel(model);
        tasksList.SetFactory(factory);





            // TODO: hanging this delegate with a this reference to a global settings extends the lifetime of MyWindow
            // Remove it on Dispose, perhaps with Event- technique?
        Application.Settings.OnChanged("filter", () => filterListModel.SetFilter(GetFilter(Application.Settings)));

        AddActions(
            new SimpleAction("remove-done-tasks", RemoveDoneTasks),
            Application.Settings.CreateAction("filter")
        );

        OnClose(_ => false.SideEffect(_ => Persistence.Save(store.GetItems<TaskItem>())));

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
    readonly ListView tasksList = null!;

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