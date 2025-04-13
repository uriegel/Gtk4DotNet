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
                            .New()
                            .Append(new Contact("Uwe Riegel", "uriegel@hotmail.de", 1965))
                            .Append(new Contact("Jim Doe", "jdoe@hotmail.de", 888))
                            .Append(new Contact("Jane Doe", "jadoe@hotmail.de", 87))
                            .Splice(3, Enumerable.Range(1, 1_000_000).Select(n => new Contact($"Item no {n}", "uriegel@hotmail.de", n)))
                            .AddWeakRef(() => Console.WriteLine("model disposed"));
            itemFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind)
                .AddWeakRef(() => Console.WriteLine("Factory disposed"));
            selectionModel = SingleSelection.New(model);

            await Task.Delay(10000);
            model.RemoveItems(10, 100);
        }

        return Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .SideEffect(InitStore)
                    .NewWindow()
                    .Title("Hello Gtk👍")
                    .DefaultSize(400, 600)
                    .Child(ScrolledWindow
                        .New()
                        .Policy(PolicyType.Never, PolicyType.Automatic)
                        .Child(ListView
                            .New(selectionModel!, itemFactory!)
                            .Ref(listViewRef)
                            .SideEffect(_ => StyleContext
                                .AddProviderForDisplay(Display.GetDefault(),
                                    CssProvider.New()
                                        .FromResource("listviewstyle"), StyleProviderPriority.Application))
                                        .AddController(EventControllerKey
                                            .New()
                                            .OnRawKeyPressed((k, Kc, m) =>
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
                                                    listViewRef?.Ref.ScrollTo(pos + 1, ListScrollFlags.ScrollFocus | ListScrollFlags.ScrollSelect, 0);
                                                    return true;
                                                }
                                                else
                                                    return false;
                                            }))))
                    .Show())
            .Run(0, IntPtr.Zero);
    }

    static readonly ObjectRef<ListViewHandle> listViewRef = new();
    static SignalListItemFactoryHandle? itemFactory;
    static SingleSelectionHandle? selectionModel; 
    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New());

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        label.Set($"{listItem.GetObject<Contact>()?.Name}");
    }
}

record Contact(string Name, string EMail, int Number);

