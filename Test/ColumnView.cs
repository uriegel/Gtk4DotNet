using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

static class ColumnViewApp
{
    public static int Run()
    {
        static void InitStore(ApplicationHandle _)
        {
            var model = ListStore
                            .New(GContact.GType)
                            .Append(GContact.New(new("Uwe Riegel", "uriegel@hotmail.de", 1965)))
                            .Append(GContact.New(new("Jim Doe", "jdoe@hotmail.de", 222)))
                            .Append(GContact.New(new("Jane Doe", "jadoe@hotmail.de", 9999)))
                            .Splice(3, [.. Enumerable.Range(1, 1000).Select(n => GContact.New(new($"Item no {n}", $"person{n}@hotmail.de", n)).Handle)])
                            .AddWeakRef(() => Console.WriteLine("model disposed"));
            var itemNameFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind)
                .AddWeakRef(() => Console.WriteLine("itemNameFactory disposed"));
            var itemEMailFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnEMailBind)
                .AddWeakRef(() => Console.WriteLine("itemEMailFactory disposed"));

            selectionModel = SingleSelection.New(model);

            col1 = ColumnViewColumn.New("Name", itemNameFactory!).AddWeakRef(() => Console.WriteLine("col1 disposed"));
            col2 = ColumnViewColumn.New("E mail", itemEMailFactory!).AddWeakRef(() => Console.WriteLine("col2 disposed"));
        }

        Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .SubClass(new GContactClass(GTypeEnum.GObject, "Contact", p => new GContact(p)))
                    .SideEffect(InitStore)
                    .NewWindow()
                    .Title("Hello Column View👍")
                    .DefaultSize(400, 600)
                    .Child(ScrolledWindow
                        .New()
                        .Policy(PolicyType.Never, PolicyType.Automatic)
                        .Child(ColumnView
                            .New(selectionModel!)
                            .AppendColumn(col1!)
                            .AppendColumn(col2!)
                            .Ref(listViewRef)
                            .SideEffect(_ => StyleContext
                                .AddProviderForDisplay(Display.GetDefault(),
                                    CssProvider.New()
                                        .FromResource("listviewstyle"), StyleProviderPriority.Application))
                            .AddController(EventControllerKey
                                .New()
                                .OnKeyPressed((k, kc, m) =>
                                {
                                    if (kc == 118)
                                    {
                                        var pos = selectionModel!.GetSelected();
                                        listViewRef?.Ref.ScrollTo(pos + 1, 0, ListScrollFlags.ScrollFocus | ListScrollFlags.ScrollSelect, 0);
                                        return true;
                                    }
                                    else
                                        return false;
                                }))))
                    .Show())
            .AddActions([new GtkAction("selend", () => Console.WriteLine("Selection till end"), "<Shift>End")])
            .Run(0, IntPtr.Zero);

        col1?.Dispose();
        col2?.Dispose();
        return 0;
    }

    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New(""));

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        label.Set(listItem.GetObject<GContact>()?.Contact?.Name);
    }

    static ColumnViewColumnHandle? col1;
    static ColumnViewColumnHandle? col2;

    static readonly ObjectRef<ColumnViewHandle> listViewRef = new();
    static SingleSelectionHandle? selectionModel; 

    static void OnEMailBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var item = listItem.GetObject<GContact>();
        label.Set(item?.Contact?.EMail);
    }
}

class GContactClass(GTypeEnum parent, string name, Func<nint, GContact> constructor)
    : SubClass<GObjectHandle>(parent, name, constructor)
{ }

class GContact(nint obj) : SubClassInst<GObjectHandle>(obj)
{
    public static GTypeHandle GType { get => _GType ?? "Contact".TypeFromName().SideEffect(n => _GType = n); }
    static GTypeHandle? _GType;

    public static GContact New(Contact contact)
    {
        using var handle = GObject.New<GObjectHandle>(GType);
        handle.IsFloating = true;
        var res = handle.GetInstance() as GContact;
        if (res != null)
            res.Contact = contact;
        return res!;
    }
    public Contact? Contact { get; set; }

    protected override GObjectHandle CreateHandle(nint obj) => new(obj);

    protected override void OnFinalize() => Console.WriteLine("Contact finalized");
}