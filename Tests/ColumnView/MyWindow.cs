using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        var store = ListStore.New()
            .Append(new Contact("Uwe Riegel", "uriegel@domain.de", 1965))
            .Append(new Contact("Jim Doe", "jdoe@domain.de", 222))
            .Append(new Contact("Jane Doe", "jadoe@domain.de", 9999));
        var items = Enumerable
            .Range(0, 1_000)
            .Select(n => new Contact($"Item no {n+1}", "uriegel@domain.de", n));
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

        var namefactory = SignalListItemFactory.New();
        namefactory.Setup(listitem => listitem.SetChild(Label.New()));
        namefactory.Bind(listitem =>
        {
            var label = listitem.GetChild<Label>();
            var item = listitem.GetItem<Contact>();
            label.Text = item?.Name ?? "";
        });
        var emailfactory = SignalListItemFactory.New();
        emailfactory.Setup(listitem => listitem.SetChild(Label.New()));
        emailfactory.Bind(listitem =>
        {
            var label = listitem.GetChild<Label>();
            var item = listitem.GetItem<Contact>();
            label.Text = item?.EMail ?? "";
        });

        columnview.SetModel(model);
        col1 = ColumnViewColumn.New("Name", namefactory);
        col2 = ColumnViewColumn.New("E mail", emailfactory);
        columnview.AppendColumn(col1);
        columnview.AppendColumn(col2);
        
        OnFinalize(() =>
        {
            col1.Dispose();
            col2.Dispose();
            model.Dispose();
        });
    }

    [Widget]
    readonly ColumnView columnview = null!;

    readonly ColumnViewColumn col1;
    readonly ColumnViewColumn col2;
}

record Contact(string Name, string EMail, int Number);
