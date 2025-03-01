using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

static class ColumnViewApp
{
    public static int Run()
    {
        void InitStore(ApplicationHandle _)
        {
            var model = ListStore
                            .New(GContact.GType)
                            .Append(GContact.New(new("Uwe Riegel", "uriegel@hotmail.de", 1965)))
                            .Append(GContact.New(new("Jim Doe", "jdoe@hotmail.de", 222)))
                            .Append(GContact.New(new("Jane Doe", "jadoe@hotmail.de", 9999)))
                            .Splice(3, [.. Enumerable.Range(1, 1000).Select(n => GContact.New(new($"Item no {n}", $"person{n}@hotmail.de", n)).Handle)]);
            itemNameFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind);
            itemEMailFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnEMailBind);

            selectionModel = SingleSelection.New(model);
        }

        return Application
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
                            .AppendColumn(ColumnViewColumn.New("Name", itemNameFactory!))
                            .AppendColumn(ColumnViewColumn.New("E mail", itemEMailFactory!))
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
    static SignalListItemFactoryHandle? itemNameFactory;
    static SignalListItemFactoryHandle? itemEMailFactory;
    static SingleSelectionHandle? selectionModel; 

    static void OnEMailBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        var oh = listItem.GetItem<GObjectHandle>();
        oh.IsFloating = true;
        var item = oh.GetInstance() as GContact;
        label.Set(item?.Contact?.EMail);
    }
}

