using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

static class ChangingColumnViews
{
    // TODO remove this example!
    // TODO only the last list view items are disposed on close
    // TODO add to selection_view
    // TODO remove from selection_view
    public static int Run()
    {
        Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .SubClass(new GContactClass(GTypeEnum.GObject, "Contact", p => new GContact(p)))
                        .SubClass(new GItem2Class(GTypeEnum.GObject, "Item2", p => new GItem2(p)))
                        .NewWindow()
                            .Titlebar(HeaderBar
                                .New()
                                .PackEnd(ToggleButton.New()
                                    .Label("Alternative Model")
                                    .OnToggled(ModelToggled)))
                            .Title("Hello Gtk👍")
                            .DefaultSize(800, 800)
                            .Child(ScrolledWindow
                                .New()
                                .Policy(PolicyType.Never, PolicyType.Automatic)
                                .Child(ColumnView
                                    .New()
                                    .Ref(columnViewRef)
                                    .SetModel1()))
                            .Show())
                .Run(0, IntPtr.Zero);

        sorter?.Dispose();
        numberSorter?.Dispose();
        textSorter?.Dispose();
        idSorter?.Dispose();
        colName?.Dispose();
        colEMail?.Dispose();
        colNumber?.Dispose();
        colID?.Dispose();
        colText?.Dispose();
        if (listModelHandle?.IsFloating != null)
            listModelHandle.IsFloating = false;
        listModelHandle?.Dispose();

        return 0;
    }

    static readonly ObjectRef<ColumnViewHandle> columnViewRef = new();
    static ColumnViewColumnHandle? colName;
    static ColumnViewColumnHandle? colEMail;
    static ColumnViewColumnHandle? colNumber;
    static ColumnViewColumnHandle? colID;
    static ColumnViewColumnHandle? colText;

    static ColumnViewHandle SetModel1(this ColumnViewHandle columnView)
    {
        textSorter?.Dispose();
        idSorter?.Dispose();
        if (colText != null)
            columnView.RemoveColumn(colText);
        colText?.Dispose();
        if (colID != null)
            columnView.RemoveColumn(colID);
        colID?.Dispose();

        var model = ListStore
                        .New(GContact.GType)
                        .Append(GContact.New(new("Uwe Riegel", "uriegel@hotmail.de", 1965)))
                        .Append(GContact.New(new("Jim Doe", "jdoe@hotmail.de", 1955)))
                        .Append(GContact.New(new("Jane Doe", "jadoe@hotmail.de", 2001)))
                        .Splice(3, [.. Enumerable.Range(1, 1000).Select(n => GContact.New(new($"Right Item no {n}", $"person{n}@hotmail.de", n)).Handle)]);

        var itemNameFactory = SignalListItemFactory
            .New()
            .AddWeakRef(() => Console.WriteLine("itemNameFactory disposed"))
            .Setup(OnListItemSetup)
            .Bind(OnListItemBind);
        var itemEMailFactory = SignalListItemFactory
            .New()
            .Setup(OnListItemSetup)
            .Bind(OnEMailBind);
        var itemNumberFactory = SignalListItemFactory
            .New()
            .Setup(OnListItemSetup)
            .Bind(OnNumberBind);


        sorter = CustomSorter.New<GObjectHandle>(NameCompare);
        numberSorter = CustomSorter.New<GObjectHandle>(NumberCompare);
        colName = ColumnViewColumn.New("Name", itemNameFactory)
                .Expand()
                .AddWeakRef(() => Console.WriteLine("ColumnViewColumn Name finalized"))
                .Resizeable()
                .SetSorter(sorter);
        colEMail = ColumnViewColumn.New("E mail", itemEMailFactory)
                .AddWeakRef(() => Console.WriteLine("ColumnViewColumn EMail finalized"))
                .Resizeable();

        colNumber = ColumnViewColumn.New("Number", itemNumberFactory)
                .Resizeable()
                .AddWeakRef(() => Console.WriteLine("ColumnViewColumn Number finalized"))
                .SetSorter(numberSorter);
        return columnView
            .AppendColumn(colName)
            .AppendColumn(colEMail)
            .AppendColumn(colNumber)
            .SideEffect(cv =>
                {
                    var sorter = cv.GetSorter();
                    var selModel = MultiSelection.New(SortListModel.New(model, sorter));
                    if (listModelHandle?.IsFloating != null)
                        listModelHandle.IsFloating = false;
                    cv.SetModel(selModel);
                    listModelHandle?.Dispose();
                    listModelHandle = model;
                });
    }

    static void SetModel2(this ColumnViewHandle columnView)
    {
        numberSorter?.Dispose();
        sorter?.Dispose();
        if (colName != null)
            columnView.RemoveColumn(colName);
        colName?.Dispose();
        if (colEMail != null)
            columnView.RemoveColumn(colEMail);
        colEMail?.Dispose();
        if (colNumber != null)
            columnView.RemoveColumn(colNumber);
        colNumber?.Dispose();

        var itemIDFactory = SignalListItemFactory
            .New()
            .Setup(OnListItemSetup)
            .Bind(OnIDBind);
        var itemTextFactory = SignalListItemFactory
            .New()
            .Setup(OnListItemSetup)
            .Bind(OnTextBind);

        var modelItem2 = ListStore
            .New(GItem2.GType)
            .Splice([.. Enumerable.Range(1, 1000).Select(n => GItem2.New(new(n, $"Item {n}")).Handle)]);

        textSorter = CustomSorter.New<GObjectHandle>(TextCompare);
        idSorter = CustomSorter.New<GObjectHandle>(IDCompare);
        colID = ColumnViewColumn.New("ID", itemIDFactory)
                .Expand()
                .Resizeable()
                .AddWeakRef(() => Console.WriteLine("ColumnViewColumn ID finalized"))
                .SetSorter(idSorter);
        colText = ColumnViewColumn.New("Text", itemTextFactory)
                .Resizeable()
                .AddWeakRef(() => Console.WriteLine("ColumnViewColumn Text finalized"))
                .SetSorter(textSorter);

        columnView
            .AppendColumn(colID)
            .AppendColumn(colText)
            .SideEffect(cv =>
                {
                    var sorter = cv.GetSorter();
                    var selModel = MultiSelection.New(SortListModel.New(modelItem2, sorter));
                    if (listModelHandle?.IsFloating != null)
                        listModelHandle.IsFloating = false;
                    cv.SetModel(selModel);
                    listModelHandle?.Dispose();
                    listModelHandle = modelItem2;
                });
    }

    static void ModelToggled(ToggleButtonHandle toggleButton)
    {
        if (toggleButton.Active())
            SetModel2(columnViewRef.Ref);
        else
            SetModel1(columnViewRef.Ref);
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

    static int TextCompare(GObjectHandle data1, GObjectHandle data2)
    {
        var c1 = data1.GetInstance() as GItem2;
        var c2 = data2.GetInstance() as GItem2;
        return string.Compare(c1?.Item2?.Text, c2?.Item2?.Text);
    }

    static int IDCompare(GObjectHandle data1, GObjectHandle data2)
    {
        var c1 = data1.GetInstance() as GItem2;
        var c2 = data2.GetInstance() as GItem2;
        return (c1?.Item2?.ID ?? 0) > (c2?.Item2?.ID ?? 0) ? 1 : -1;
    }

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

    static CustomSorterHandle? sorter;
    static CustomSorterHandle? numberSorter;
    static CustomSorterHandle? textSorter;
    static CustomSorterHandle? idSorter;
    static IListModel? listModelHandle;

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

    static void OnIDBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GItem2;
        label.Set($"{item?.Item2?.ID}");
    }

    static void OnTextBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GItem2;
        label.Set(item?.Item2?.Text);
    }
}

record Item2(int ID, string Text);

class GItem2Class(GTypeEnum parent, string name, Func<nint, GItem2> constructor)
    : SubClass<GObjectHandle>(parent, name, constructor)
{ }

class GItem2(nint obj) : SubClassInst<GObjectHandle>(obj)
{
    public static GTypeHandle GType { get => _GType ?? "Item2".TypeFromName().SideEffect(n => _GType = n); }
    static GTypeHandle? _GType;

    public static GItem2 New(Item2 item2)
    {
        using var handle = GObject.New<GObjectHandle>(GType);
        handle.IsFloating = true;
        var res = handle.GetInstance() as GItem2;
        if (res != null)
            res.Item2 = item2;
        return res!;
    }
    public Item2? Item2 { get; set; }

    protected override GObjectHandle CreateHandle(nint obj) => new(obj);

    protected override void OnFinalize() => Console.WriteLine("Item2 finalized");
}