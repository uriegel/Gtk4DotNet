using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        var store = ListStore.New();
        var items = Enumerable
            .Range(0, 100_000)
            .Select(n => new Item(n + 1));
        foreach (var item in items)
            store.Append(item);

        // Simple SingelSelection model
        var model = SingleSelection.New(store);

        // SingleSelection with filtering
        // var filter = CustomFilter.New<Item>(item => (item?.Number ?? 0)  % 2 == 0);
        // var model = SingleSelection.New(FilterListModel.New(store, filter));

        // SingleSelection with sorting
        // var sorter = CustomSorter.New<Item>((item1, item2) => (item2?.Number ?? 0) - (item1?.Number ?? 0));
        // var model = SingleSelection.New(SortListModel.New(store, sorter));

        // SingleSelection with sorting and filtering
        // var sorter = CustomSorter.New<Item>((item1, item2) => (item2?.Number ?? 0) - (item1?.Number ?? 0));
        // var filter = CustomFilter.New<Item>(item => (item?.Number ?? 0)  % 2 == 0);
        // var model = SingleSelection.New(SortListModel.New(FilterListModel.New(store, filter), sorter));

        var factory = SignalListItemFactory.New();
        factory.Setup(listitem =>
        {
            listitem.SetChild(Label.New());
        });
        factory.Bind(listitem =>
        {
            var label = listitem.GetChild<Label>();
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
