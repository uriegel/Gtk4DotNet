using System.ComponentModel;
using CsTools.Extensions;
using Gtk4DotNet;

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
            new SimpleAction("find", FindItem),
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

        columnviewLeft.OnActivate += pos => Console.WriteLine($"Activated {pos} item");

        colAdapter = new(paned, scrolledLeft, scrolledRight,
            () =>
            {
                using var cols = columnviewLeft.GetColumns();
                cols.FirstOrDefault()?.Title = "N";
                cols.Skip(1).FirstOrDefault()?.Title = "E";
            },
            () =>
            {
                using var cols = columnviewLeft.GetColumns();
                cols.FirstOrDefault()?.Title = "Name";
                cols.Skip(1).FirstOrDefault()?.Title = "E Mail ( back again)";
            });
        colAdapter2 = new(paned, scrolledRight, scrolledLeft,
            () =>
            {
                using var cols = columnviewRight.GetColumns();
                cols.FirstOrDefault()?.Title = "N";
                cols.Skip(1).FirstOrDefault()?.Title = "E";
            },
            () =>
            {
                using var cols = columnviewRight.GetColumns();
                cols.FirstOrDefault()?.Title = "Name";
                cols.Skip(1).FirstOrDefault()?.Title = "E Mail ( back again)";
            });

        OnClose(async _ =>
        {
            inChange = false;
            await Task.Delay(200);
            return false;
        });

        OnFinalize(() =>
        {
            model.Dispose();
        });
    }

    async void ToggleModel(ColumnView columnview, bool newModel)
    {
        GC.Collect();
        GC.Collect();
        inChange = false;
        if (!newModel)
        {
            var store = ListStore.New()
                .Append(new Contact("Uwe Riegel", "riegel@domain.de", 1965, "mail-read"))
                .Append(new Contact("Jim Doe", "jdoe@domain.de", 222, "mail-unread"))
                .Append(new Contact("Jane Doe", "zjadoe@domain.de", 9999, "mail"));
            var oldModel = model;
            sortModel = SortListModel.New(store, null);
            model = NoSelection.New(sortModel);
            oldModel?.Dispose();

            var namefactory = SignalListItemFactory
                .New()
                .Setup(listitem =>
                {
                    using var builder = Builder.FromDotNetResource("iconnameitem");
                    var item = new IconNameItem(builder);
                    listitem.SetManagedChild(item);
                })
                .Bind(listitem =>
                {
                    var iconname = listitem.GetManagedChild<IconNameItem>();
                    var item = listitem.GetItem<Contact>();
                    iconname?.Name = item?.Name ?? "";
                    if (item?.IconName != null)
                        iconname?.SetFromIconName(item.IconName);
                });
            var emailfactory = SignalListItemFactory
                .New()
                .Setup(listitem => listitem.SetChild(Label.New().SetEllipsize(EllipsizeMode.End)))
                .Bind(listitem =>
                {
                    var label = listitem.GetChild<Label>();
                    var item = listitem.GetItem<Contact>();
                    label.Text = item?.EMail ?? "";
                });

            columnview.SetModel(null);
            columnview.ClearColumns();
            columnview.SetModel(model);
            using var nameSorter = CustomSorter.New<Contact>((item1, item2) => (item1?.Name ?? "").CompareTo(item2?.Name ?? ""));
            using var mailSorter = CustomSorter.New<Contact>((item1, item2) => (item1?.EMail ?? "").CompareTo(item2?.EMail ?? ""));
            columnview.AppendColumn(ColumnViewColumn.New("Name", namefactory).SideEffect(cvc => cvc.SetSorter(nameSorter)));
            columnview.AppendColumn(ColumnViewColumn.New("E mail address (long column)", emailfactory).Expand().SideEffect(cvc => cvc.SetSorter(mailSorter)));
            var viewsorter = columnview.GetSorter();
            viewsorter?.OnChanged -= SortOrderChanged;
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

            var namefactory = SignalListItemFactory.New()
                .Setup(listitem => listitem.SetChild(Label.New()))
                .Bind(listitem =>
                {
                    var label = listitem.GetChild<Label>();
                    var item = listitem.GetItem<Item>();
                    label.DataContext = item;
                    label.SetBinding("label", nameof(item.Name));
                })
                .Unbind(listitem =>
                {
                    var label = listitem.GetChild<Label>();
                    label.UnsetBinding("label");
                    label.DataContext = null;
                });

            columnview.SetModel(null);
            columnview.ClearColumns();
            columnview.SetModel(null);
            var items = Enumerable
                .Range(0, 10_000)
                .Select(n => new Item($"Item no {n + 1}", n));
            store.Splice(0, 0, items);

            var sorterIsEven = CustomSorter.New<Item>((item1, item2) =>
            {
                var order = (item1?.Number ?? 0) % 2 - (item2?.Number ?? 0) % 2;
                return reverseSortOrder ? -order : order;
            });
            var sorter = CustomSorter.New<Item>((item1, item2) => (item1?.Number ?? 0) - (item2?.Number ?? 0));
            using var multiSorter = MultiSorter
                .New()
                .Append(sorterIsEven)
                .Append(sorter);

            var col = ColumnViewColumn.New("Name", namefactory).Expand().SideEffect(cvc => cvc.SetSorter(multiSorter));
            columnview.AppendColumn(col);
            columnview.SortByColumn(col);
            columnview.SetModel(model);
            var viewsorter = columnview.GetSorter();
            viewsorter.OnChanged += SortOrderChanged;
            sortModel.SetSorter(viewsorter);

            await Changer();        

            async Task Changer()
            {
                 var item = model.GetItem<Item>(10);
                inChange = true;
                for (var i = 1; i < 100_000 && inChange; i++)
                {
                    item?.Name = $"Eintrag {i}";
                    await Task.Delay(40);
                }
                inChange = false;
            }
        }
        columnview.GetModel()?.UnselectAll();
    }
    
    void SortOrderChanged(bool reverse, ColumnViewColumn? _,  SorterChange __) 
    {
        reverseSortOrder = reverse;
        Console.WriteLine($"Ordering reverse: {reverse}");
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

    void FindItem()
    {
        var items = model.GetItems<Item>().Take(20);
        foreach (var item in items)
            Console.WriteLine(item.Name);
    }

    ColumnView GetInactiveView()
        => columnviewLeft == activeView ? columnviewRight : columnviewLeft;

    [Widget]
    readonly ColumnView columnviewLeft = null!;

    [Widget]
    readonly ColumnView columnviewRight = null!;

    [Widget]
    readonly ScrolledWindow scrolledLeft = null!;

    [Widget]
    readonly ScrolledWindow scrolledRight = null!;

    ColumnView? activeView;
    ColumnView lastActiveView = null!;

    [Widget]
    readonly Paned paned = null!;

    SelectionModel model = null!;

    CustomFilter filterNumbers = null!;

    SortListModel sortModel = null!;

    ColumnViewColAdapter colAdapter;
    ColumnViewColAdapter colAdapter2;

    bool filter;

    bool reverseSortOrder;
    bool inChange;
}
record Contact(string Name, string EMail, int Number, string IconName);

class Item(string name, int number) : INotifyPropertyChanged
{
    //~Item() { Console.WriteLine("Item destroyed"); }
    public string Name
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnChanged(nameof(Name));
            }
        }
    } = name;

    public int Number { get => number; }

    public event PropertyChangedEventHandler? PropertyChanged;

    void OnChanged(string name) => PropertyChanged?.Invoke(this, new(name));
}
