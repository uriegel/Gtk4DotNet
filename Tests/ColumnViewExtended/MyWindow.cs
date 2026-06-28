using CsTools.Extensions;
using Gtk4DotNet;

// TODO Ins to toggle and move next, set selection to all and none, shift pos/end
// TODO Mouse click must remain selection

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        StyleContext.AddProviderForDisplay(
            Display.GetDefault(),
            CssProvider.New().FromResource("style"),
            StyleProviderPriority.Application);

        ToggleModel(columnviewLeft, false);
        ToggleModel(columnviewRight, false);
        AddActions(
            new BoolAction("new-model", false, newModel => ToggleModel(columnviewLeft, newModel), "F3"),
            new BoolAction("filter", false, FilterModel, "<Ctrl>F"),
            new SimpleAction("quit", CloseWindow, "<Ctrl>Q")
        );
        activeView = columnviewLeft;

        var keyController = KeyEventController.New();
        keyController.OnKeyPressed += (chr, key) =>
        {
            if (chr == (char)ConsoleKey.Tab && !key.HasFlag(KeyModifiers.Shift))
            {
                GetInactiveView()?.GrabFocus();
                return true;
            }
            else
                return false;
        };
        paned.AddController(keyController);
        var leftEvents = FocusEventController.New();
        leftEvents.OnEnter += () =>
        {
            activeView = columnviewLeft;
            lastActiveView = columnviewLeft;
        };
        leftEvents.OnLeave += () => activeView = null;

        var rightEvents = FocusEventController.New();
        rightEvents.OnEnter += () =>
        {
            activeView = columnviewRight;
            lastActiveView = columnviewRight;
        };
        rightEvents.OnLeave += () => activeView = null;
        columnviewLeft.AddController(leftEvents);
        columnviewRight.AddController(rightEvents);

        var kec = KeyEventController.New();
        kec.SetPropagationPhase(PropagationPhase.Capture);
        kec.OnKeyPressed += (chr, mod) => OnKey(activeView, chr);
        AddController(kec);

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

            columnview.SetModel(null);
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
            filter = false;
            var store = ListStore.New();
            var oldModel = model;
            filterNumbers = CustomFilter.New<Item>(item => !filter || (item?.Number ?? 0) % 2 == 0);
            sortModel = SortListModel.New(FilterListModel.New(store, filterNumbers), null);
            model = MultiSelection.New(sortModel);
            oldModel?.Dispose();

            var namefactory = SignalListItemFactory.New();
            namefactory.Setup(listitem => listitem.SetChild(Label.New()));
            namefactory.Bind(listitem =>
            {
                var label = listitem.GetChild<Label>();
                var item = listitem.GetItem<Item>();
                label.Text = item?.Name ?? "";
            });

            columnview.SetModel(null);
            columnview.ClearColumns();
            columnview.SetModel(null);
            var items = Enumerable
                .Range(0, 100_000)
                .Select(n => new Item($"Item no {n + 1}", n));
            foreach (var item in items)
                store.Append(item);
            using var sorter = CustomSorter.New<Item>((item1, item2) => (item1?.Number ?? 0) - (item2?.Number ?? 0));
            columnview.AppendColumn(ColumnViewColumn.New("Name", namefactory).Expand().SideEffect(cvc => cvc.SetSorter(sorter)));
            columnview.SetModel(model);
            using var viewsorter = columnview.GetSorter();
            sortModel.SetSorter(viewsorter);
        }
        columnview.GetModel()?.UnselectAll();
    }

    void FilterModel(bool filter)
    {
        this.filter = filter;
        filterNumbers.Changed(filter ? FilterChange.MoreStrict : FilterChange.LessStrict);
    }

    bool OnKey(ColumnView? view, char key)
    {
        if (view == null)
            return false;
        switch (key)
        {
            case (char)ConsoleKey.UpArrow:
            case (char)ConsoleKey.DownArrow:
                var pos = view.GetFocusedItemPos();
                var newPos = key switch
                {
                    (char)ConsoleKey.UpArrow => Math.Max(pos - 1, 0),
                    (char)ConsoleKey.DownArrow => Math.Min(pos + 1, view.ItemsCount() - 1),
                    _ => 0
                };
                view.ScrollTo(newPos, ListScrollFlags.ScrollFocus);
                return true;
            case (char)ConsoleKey.Home:
                view.ScrollTo(0, ListScrollFlags.ScrollFocus);
                return true;
            case (char)ConsoleKey.End:
                view.ScrollTo(view.ItemsCount() - 1, ListScrollFlags.ScrollFocus);
                return true;
            case (char)ConsoleKey.PageUp:
            case (char)ConsoleKey.PageDown:
                var pageSize = GetNumberOfVisibleRows(view);
                pos = view.GetFocusedItemPos();
                newPos = key switch
                {
                    (char)ConsoleKey.PageUp => Math.Max(pos - pageSize, 0),
                    (char)ConsoleKey.PageDown => Math.Min(pos + pageSize, view.ItemsCount() - 1),
                    _ => 0
                };
                view.ScrollTo(newPos, ListScrollFlags.ScrollFocus);
                return true;
        }
        return false;
    }

    int GetNumberOfVisibleRows(ColumnView? view)
    {
        if (view == null)
            return 0;
        var row = GetFocus<Widget>();
        if (!row.IsInvalid && row.WidgetName == "GtkColumnViewRowWidget")
            return (view.Height / (row.Height + 1)) - 4;
        else
            return 0;
    }

    ColumnView GetInactiveView()
        => columnviewLeft == activeView ? columnviewRight : columnviewLeft;

    [Widget]
    readonly ColumnView columnviewLeft = null!;

    [Widget]
    readonly ColumnView columnviewRight = null!;

    ColumnView? activeView;
    ColumnView lastActiveView = null!;

    [Widget]
    readonly Widget paned = null!;

    SelectionModel model = null!;

    CustomFilter filterNumbers = null!;

    SortListModel sortModel = null!;

    bool filter;
}
record Contact(string Name, string EMail, int Number, string IconName);
record Item(string Name, int Number);
