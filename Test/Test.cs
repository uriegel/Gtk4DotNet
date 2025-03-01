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
                            .Append(GContact.New(new("Uwe Riegel", "uriegel@hotmail.de")))
                            .Append(GContact.New(new("Jim Doe", "jdoe@hotmail.de")))
                            .Append(GContact.New(new("Jane Doe", "jadoe@hotmail.de")))
                            .Splice(3, [.. Enumerable.Range(1, 1000).Select(n => GContact.New(new($"Left Item no {n}", $"person{n}@hotmail.de")).Handle)]);
            var model2 = ListStore
                            .New(GContact.GType)
                            .Splice([.. Enumerable.Range(1, 1000).Select(n => GContact.New(new($"Right Item no {n}", $"person{n}@hotmail.de")).Handle)]);
            itemNameFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind);
            itemEMailFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnEMailBind);

            selectionModel1 = SingleSelection.New(model1);
            selectionModel2 = MultiSelection.New(SortListModel.New(model2, sorter));
        }

        return Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .SubClass(new GContactClass(GTypeEnum.GObject, "Contact", p => new GContact(p)))
                        .SideEffect(InitStore)
                        .NewWindow()
                            .Title("Hello Gtk👍")
                            .DefaultSize(600, 800)
                            .Child(Paned
                                .New(Orientation.Horizontal)
                                .StartChild(ScrolledWindow
                                    .New()
                                    .Policy(PolicyType.Never, PolicyType.Automatic)
                                    .Child(ColumnView
                                        .New(selectionModel1!)
                                        .AppendColumn(ColumnViewColumn.New("Name", itemNameFactory!))
                                        .AppendColumn(ColumnViewColumn.New("E mail", itemEMailFactory!))), true, true)
                                .EndChild(ScrolledWindow
                                    .New()
                                    .Policy(PolicyType.Never, PolicyType.Automatic)
                                    .Child(ColumnView
                                        .New(selectionModel2!)
                                        .AppendColumn(ColumnViewColumn.New("Name", itemNameFactory!)
                                            .SetSorter(sorter))
                                        .AppendColumn(ColumnViewColumn.New("E mail", itemEMailFactory!))), true, true))
                            .Show())
                .Run(0, IntPtr.Zero);
    }

    static int NameCompare(GObjectHandle data1, GObjectHandle data2)
    {
        var c1 = data1.GetInstance() as GContact;
        var c2 = data2.GetInstance() as GContact;
        return - string.Compare(c1?.Contact?.Name, c2?.Contact?.Name);
    }

    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New(""));

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GContact;
        label.Set(item?.Contact?.Name);
    }

    static readonly ObjectRef<ColumnViewHandle> listViewRef = new();

    static CustomSorterHandle sorter = CustomSorter.New<GObjectHandle>(NameCompare);
    static SignalListItemFactoryHandle? itemNameFactory;
    static SignalListItemFactoryHandle? itemEMailFactory;
    static SingleSelectionHandle? selectionModel1; 
    static IListModel? selectionModel2; 

    static void OnEMailBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GContact;
        label.Set(item?.Contact?.EMail);
    }
}
