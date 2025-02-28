using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

static class CustomItemListView
{
    public static int Run()
    {
        async void InitStore(ApplicationHandle _)
        {
            var model = ListStore
                            .New(GContact.GType)
                            .Append(GContact.New(new("Uwe Riegel", "uriegel@hotmail.de")))
                            .Append(GContact.New(new("Jim Doe", "jdoe@hotmail.de")))
                            .Append(GContact.New(new("Jane Doe", "jadoe@hotmail.de")))
                            .Splice(3, [.. Enumerable.Range(1, 10_000_000).Select(n => GContact.New(new($"Item no {n}", "uriegel@hotmail.de")).Handle)]);
            itemFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind);
            selectionModel = SingleSelection.New(model);

            await Task.Delay(10000);
            model.RemoveItems(10, 100);
        }

        return Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .SubClass(new GContactClass(GTypeEnum.GObject, "Contact", p => new GContact(p)))
                    .SideEffect(InitStore)
                    .NewWindow()
                    .Title("Hello Gtk👍")
                    .DefaultSize(400, 600)
                    .Child(ScrolledWindow
                        .New()
                        .Policy(PolicyType.Never, PolicyType.Automatic)
                        .Child(ListView
                            .New(selectionModel!, itemFactory!)
                            .SideEffect(w => StyleContext
                                .AddProviderForDisplay(Display.GetDefault(),
                                    CssProvider.New()
                                        .FromResource("listviewstyle"), StyleProviderPriority.Application))
                                        .AddController(EventControllerKey
                                            .New()
                                            .OnKeyPressed((k, Kc, m) =>
                                            {
                                                if ((m & KeyModifiers.Control) == KeyModifiers.Control)
                                                {
                                                    if (Kc == 115)
                                                        return true;
                                                    else
                                                        return false;
                                                }
                                                else if (Kc == 118)
                                                {
                                                    var pos = selectionModel!.GetSelected();
                                                    selectionModel!.SetSelected(pos + 1);
                                                    return true;
                                                }
                                                else
                                                    return false;
                                            }))))
                    .Show())
            .Run(0, IntPtr.Zero);
    }

    static SignalListItemFactoryHandle? itemFactory;
    static SingleSelectionHandle? selectionModel; 
    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New(""));

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GContact;
        label.Set(item?.Contact?.Name);
    }
}

record Contact(string Name, string EMail);

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
/*
public static class THandleExtensions
{
    public static GContact GetInstance(this GObjectHandle handle)
        => handle.GetInternalHandle().GetInstance();
    
}
*/