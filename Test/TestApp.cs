using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

static class TestApp
{
    public static int Run()
    {
        void InitStore(ApplicationHandle _)
        {
            var model1 = ListStore
                            .New(GContact.GType)
                            .Splice([.. Enumerable.Range(1, 1000).Select(n => GContact.New(new($"Left Item no {n}", $"person{n}@hotmail.de", n)).Handle)]);
            model2 = ListStore
                            .New(GContact.GType)
                            .Append(GContact.New(new("Uwe Riegel", "uriegel@hotmail.de", 1965)))
                            .Append(GContact.New(new("Jim Doe", "jdoe@hotmail.de", 1955)))
                            .Append(GContact.New(new("Jane Doe", "jadoe@hotmail.de", 2001)))
                            .Splice(3, [.. Enumerable.Range(1, 1000).Select(n => GContact.New(new($"Right Item no {n}", $"person{n}@hotmail.de", n)).Handle)]);
            itemNameFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind);
            itemEMailFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnEMailBind);
            itemNumberFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnNumberBind);

            selectionModel1 = SingleSelection.New(model1);
        }

        return Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .SubClass(new GContactClass(GTypeEnum.GObject, "Contact", p => new GContact(p)))
                        .SideEffect(InitStore)
                        .NewWindow()
                            .Titlebar(HeaderBar
                                .New()
                                .PackEnd(ToggleButton.New()
                                    .Label("Action")
                                    .OnToggled(ActionEnabled)))
                            .Title("Hello Gtk👍")
                            .DefaultSize(800, 800)
                            .Child(Paned
                                .New(Orientation.Horizontal)
                                .StartChild(ScrolledWindow
                                    .New()
                                    .Policy(PolicyType.Never, PolicyType.Automatic)
                                    .Child(ColumnView
                                        .New(selectionModel1!)
                                        .AppendColumn(ColumnViewColumn.New("Name", itemNameFactory!).Expand())
                                        .AppendColumn(ColumnViewColumn.New("E mail", itemEMailFactory!))), true, true)

                                .EndChild(ScrolledWindow
                                    .New()
                                    .Policy(PolicyType.Never, PolicyType.Automatic)
                                    .Child(ColumnView
                                        .New()
                                        .AppendColumn(ColumnViewColumn.New("Name", itemNameFactory!)
                                            .Expand()
                                            .Resizeable()
                                            .SetSorter(sorter))
                                        .AppendColumn(ColumnViewColumn.New("E mail", itemEMailFactory!)
                                            .Resizeable())
                                        .AppendColumn(ColumnViewColumn.New("Number", itemNumberFactory!)
                                            .Resizeable()
                                            .SetSorter(numberSorter))
                                        .SideEffect(cv =>
                                            {
                                                var sorter = cv.GetSorter();
                                                var model = MultiSelection.New(SortListModel.New(model2!, sorter));
                                                cv.SetModel(model);
                                            })
                                        .AddController(EventControllerFocus.New()
                                            .OnEnter(() => IActionMap.GetAction("down").SetEnabled(true))
                                            .OnLeave(() => IActionMap.GetAction("down").SetEnabled(false)))
                                        ), true, true))
                            .Show())
                .AddActions([new GtkAction("down", () => Console.WriteLine("Down"), "Down")])
                .AddActions([new GtkAction("tab", () => Console.WriteLine("Tab"), "Tab")])
                .SideEffect(a => IActionMap.GetAction("down").SetEnabled(false))
                .Run(0, IntPtr.Zero);
    }

    static int NameCompare(GObjectHandle data1, GObjectHandle data2)
    {
        var c1 = data1.GetInstance() as GContact;
        var c2 = data2.GetInstance() as GContact;
        return string.Compare(c1?.Contact?.Name, c2?.Contact?.Name);
    }

    static int NumberCompare(GObjectHandle data1, GObjectHandle data2)
    {
        var c1 = data1.GetInstance() as GContact;
        var c2 = data2.GetInstance() as GContact;
        return (c1?.Contact?.Number ?? 0) > (c2?.Contact?.Number ?? 0) ? 1 : -1;
    }

    static void ActionEnabled(ToggleButtonHandle toggleButton)
        => IActionMap.GetAction("down").SetEnabled(toggleButton.Active());

    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New("").HAlign(Align.Start));

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GContact;
        label.Set(item?.Contact?.Name);
    }

    static readonly ObjectRef<ColumnViewHandle> listViewRef = new();

    static IListModel? model2;
    static CustomSorterHandle sorter = CustomSorter.New<GObjectHandle>(NameCompare);
        static CustomSorterHandle numberSorter = CustomSorter.New<GObjectHandle>(NumberCompare);
    static SignalListItemFactoryHandle? itemNameFactory;
    static SignalListItemFactoryHandle? itemEMailFactory;
    static SignalListItemFactoryHandle? itemNumberFactory;
    static SingleSelectionHandle? selectionModel1;

    static void OnEMailBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GContact;
        label.Set(item?.Contact?.EMail);
    }
    
    static void OnNumberBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GContact;
        label.Set($"{item?.Contact?.Number}");
    }
}
