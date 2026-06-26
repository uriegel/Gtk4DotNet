using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        ToggleModel(false);
        AddActions(
            new BoolAction("preview", false, ToggleModel, "F3"),
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q")
        );

        OnFinalize(() =>
        {
            model.Dispose();
        });
    }

    void ToggleModel(bool newModel)
    {
        if (!newModel)
        {
            var store = ListStore.New()
                .Append(new Contact("Uwe Riegel", "uriegel@domain.de", 1965))
                .Append(new Contact("Jim Doe", "jdoe@domain.de", 222))
                .Append(new Contact("Jane Doe", "jadoe@domain.de", 9999));
            var oldModel = model;
            model = SingleSelection.New(store);
            oldModel?.Dispose();

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
            columnview.ClearColumns();
            columnview.AppendColumn(ColumnViewColumn.New("Name", namefactory));
            columnview.AppendColumn(ColumnViewColumn.New("E mail", emailfactory).Expand());
        }
        else
        {
            var store = ListStore.New();
            var oldModel = model;
            model = SingleSelection.New(store);
            oldModel?.Dispose();
            var items = Enumerable
                .Range(0, 100_000)
                .Select(n => new Contact($"Item no {n + 1}", "uriegel@domain.de", n));
            foreach (var item in items)
                store.Append(item);

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
            columnview.ClearColumns();
            columnview.AppendColumn(ColumnViewColumn.New("Name", namefactory));
            columnview.AppendColumn(ColumnViewColumn.New("E mail", emailfactory).Expand());
        }
    }

    [Widget]
    readonly ColumnView columnview = null!;

    SelectionModel model = null!;
}
record Contact(string Name, string EMail, int Number);
