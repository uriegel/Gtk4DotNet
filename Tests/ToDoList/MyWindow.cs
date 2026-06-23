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
            listitem.SetChild(Label.New());
        });
        factory.Bind(listitem =>
        {
            var label = listitem.GetChild<Label>();
            var item = listitem.GetItem<Task>();
            label.Text = $"Item #{item?.Content}";
        });

        tasksList.SetModel(model);
        tasksList.SetFactory(factory);

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
        store.Append(new Task(false, text));
    }

    [Widget]
    readonly ListView tasksList = null!;

    [Widget]
    readonly Entry entry = null!;

    readonly ListStore store;
}

record Task(bool Completed, string Content);