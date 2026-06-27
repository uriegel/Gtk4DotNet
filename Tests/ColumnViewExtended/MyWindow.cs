using CsTools.Extensions;
using Gtk4DotNet;

// TODO FocusEvetnController with events
// TODO keep multi selection permanent (click with mouse and space)
// TODO Shortcut actions like up, down, pageup, pagedoen, but only for the group? test witf another ListBox

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        ToggleModel(columnviewLeft, false);
        ToggleModel(columnviewRight, false);
        AddActions(
            new BoolAction("new-model", false, newModel => ToggleModel(columnviewLeft, newModel), "F3"),
            new BoolAction("filter", false, FilterModel, "<Ctrl>F"),
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q")
        );
        activeView = columnviewLeft;

        paned.AddController(KeyEventController.New().OnKeyPressed((chr, key) =>
        {
            if (chr == (char)ConsoleKey.Tab && !key.HasFlag(KeyModifiers.Shift))
            {
                GetInactiveView()?.GrabFocus();
                return true;
            }
            else
                return false;
        }));
        columnviewLeft.AddController(FocusEventController.New()
            .OnEnter(() => activeView = columnviewLeft)
            .OnLeave(() => { }));
        columnviewRight.AddController(FocusEventController.New()
            .OnEnter(() => activeView = columnviewRight)
            .OnLeave(() => { }));

        OnFinalize(() =>
        {
            model.Dispose();
        });
    }

    void ToggleModel(ColumnView columnview, bool newModel)
    {
        if (!newModel)
        {
            var store = ListStore.New()
                .Append(new Contact("Uwe Riegel", "riegel@domain.de", 1965, "mail-read"))
                .Append(new Contact("Jim Doe", "jdoe@domain.de", 222, "mail-unread"))
                .Append(new Contact("Jane Doe", "zjadoe@domain.de", 9999, "mail"));
            var oldModel = model;
            sortModel = SortListModel.New(store, null);
            model = SingleSelection.New(sortModel);
            oldModel?.Dispose();

            var namefactory = SignalListItemFactory.New();
            namefactory.Setup(listitem =>
            {
                using var builder = Builder.FromDotNetResource("iconnameitem");
                var item = new IconNameItem(builder);
                listitem.SetManagedChild(item);
            });
            namefactory.Bind(listitem =>
            {
                var iconname = listitem.GetManagedChild<IconNameItem>();
                var item = listitem.GetItem<Contact>();
                iconname?.Name = item?.Name ?? "";
                if (item?.IconName != null)
                    iconname?.SetFromIconName(item.IconName);
            });
            var emailfactory = SignalListItemFactory.New();
            emailfactory.Setup(listitem => listitem.SetChild(Label.New()));
            emailfactory.Bind(listitem =>
            {
                var label = listitem.GetChild<Label>();
                var item = listitem.GetItem<Contact>();
                label.Text = item?.EMail ?? "";
            });

            columnview.ClearColumns();
            columnview.SetModel(model);
            using var nameSorter = CustomSorter.New<Contact>((item1, item2) => (item1?.Name ?? "").CompareTo((item2?.Name ?? "")));
            using var mailSorter = CustomSorter.New<Contact>((item1, item2) => (item1?.EMail ?? "").CompareTo((item2?.EMail ?? "")));
            columnview.AppendColumn(ColumnViewColumn.New("Name", namefactory).SideEffect(cvc => cvc.SetSorter(nameSorter)));
            columnview.AppendColumn(ColumnViewColumn.New("E mail", emailfactory).Expand().SideEffect(cvc => cvc.SetSorter(mailSorter)));
            using var viewsorter = columnview.GetSorter();
            sortModel.SetSorter(viewsorter);
        }
        else
        {
            this.filter = false;
            var store = ListStore.New();
            var oldModel = model;
            filterNumbers = CustomFilter.New<Item>(item => !filter || (item?.Number ?? 0) % 2 == 0);
            model = MultiSelection.New(FilterListModel.New(store, filterNumbers));
            oldModel?.Dispose();

            var namefactory = SignalListItemFactory.New();
            namefactory.Setup(listitem => listitem.SetChild(Label.New()));
            namefactory.Bind(listitem =>
            {
                var label = listitem.GetChild<Label>();
                var item = listitem.GetItem<Item>();
                label.Text = item?.Name ?? "";
            });

            columnview.ClearColumns();
            columnview.SetModel(null);
            var items = Enumerable
                .Range(0, 100_000)
                .Select(n => new Item($"Item no {n + 1}", n));
            foreach (var item in items)
                store.Append(item);
            columnview.AppendColumn(ColumnViewColumn.New("Name", namefactory).Expand());
            columnview.SetModel(model);
        }
    }

    void FilterModel(bool filter)
    {
        this.filter = filter;
        filterNumbers.Changed(filter ? FilterChange.MoreStrict : FilterChange.LessStrict);
    }

    ColumnView GetInactiveView()
        => columnviewLeft == activeView ? columnviewRight : columnviewLeft;

    [Widget]
    readonly ColumnView columnviewLeft = null!;

    [Widget]
    readonly ColumnView columnviewRight = null!;

    ColumnView activeView = null!;

    [Widget]
    readonly Widget paned = null!;

    SelectionModel model = null!;

    CustomFilter filterNumbers = null!;

    SortListModel sortModel = null!;

    bool filter;
}
record Contact(string Name, string EMail, int Number, string IconName);
record Item(string Name, int Number);
