using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        var store = new ListStore<Item>();
        var items = Enumerable
            .Range(0, 100_000)
            .Select(n => new Item(n + 1));
        foreach (var item in items)
            store.Append(item);

        // Simple SingelSelection model
        var model = new SingleSelection(store);

        // SingleSelection with filtering
        // var filter = CustomFilter.New<Item>(item => (item?.Number ?? 0)  % 2 == 0);
        // var model = new SingleSelection(new FilterListModel(store, filter));

        // SingleSelection with sorting
        // var sorter = CustomSorter.New<Item>((item1, item2) => (item2?.Number ?? 0) - (item1?.Number ?? 0));
        // var model = new SingleSelection(new SortListModel(store, sorter));

        // SingleSelection with sorting and filtering
        // var sorter = CustomSorter.New<Item>((item1, item2) => (item2?.Number ?? 0) - (item1?.Number ?? 0));
        // var filter = CustomFilter.New<Item>(item => (item?.Number ?? 0)  % 2 == 0);
        // var model = new SingleSelection(new SortListModel(new FilterListModel(store, filter), sorter));

        var factory = new SignalListItemFactory()
            .Setup(listitem => listitem.SetChild(new Label()))
            .Bind(listitem =>
            {
                var label = listitem.GetChild().AsLabel();
                var item = listitem.GetItem<Item>();
                label.Text = $"Item #{item?.Number}";
            });

        listview.SetModel(model);
        listview.SetFactory(factory);

        OnFinalize(() =>
        {
            factory.Dispose();
            model.Dispose();
        });
    }

    [Widget]
    readonly ListView listview = null!;
}

record Item(int Number)
{
    // ~Item() => Console.WriteLine("Item destroyed");
}
