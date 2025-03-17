using System.Diagnostics;
using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.SafeHandles;

using static System.Console;

static class ColumnViewCleanup
{
    public static void Check()
    {
        var count = 1_000_000;

        static void InitStore(ApplicationHandle _)
        {
            model = ListStore.New().AddWeakRef(() => WriteLine("model disposed"));
            itemNameFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind)
                .AddWeakRef(() => WriteLine("itemNameFactory disposed"));
            itemEMailFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnEMailBind)
                .AddWeakRef(() => WriteLine("itemEMailFactory disposed"));

            selectionModel = SingleSelection.New(model).AddWeakRef(() => WriteLine("SelectionModel disposed"));

            col1 = ColumnViewColumn.New("Name", itemNameFactory!).AddWeakRef(() => WriteLine("col1 disposed"));
            col2 = ColumnViewColumn.New("E mail", itemEMailFactory!).AddWeakRef(() => WriteLine("col2 disposed"));
        }


        int schritt = 0;

        Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .SideEffect(InitStore)
                    .NewWindow()
                    .SideEffect(MemoryChecker)
                    .Title("Hello Gtk👍")
                    .DefaultSize(400, 600)
                    .Child(ScrolledWindow
                        .New()
                        .Ref(scrolledWindow)
                        .Policy(PolicyType.Never, PolicyType.Automatic)
                        .Child(ColumnView
                            .New(selectionModel)
                            .AppendColumn(col1!)
                            .AppendColumn(col2!)))
                    .Show())
            .Run(0, IntPtr.Zero);

        void MemoryChecker(WindowHandle w)
        {
            w.SetTimer(300, TimeSpan.FromSeconds(5), () =>
            {
                PrintMemory();
                var now = DateTime.Now;
                if (schritt != 20 && schritt != 21 && schritt % 2 == 1)
                    model?.Splice(0, 0, Enumerable
                               .Range(1, count)
                               .Select(n => new Contact($"Item no {n}", "uriegel@hotmail.de", n)));

                var stamp = DateTime.Now - now;
                WriteLine($"Duration: {stamp}");

                if (schritt != 20 && schritt != 21 && schritt > 0 && schritt % 2 == 0)
                    model?.RemoveItems(0, (uint)count);

                if (schritt == 20)
                    scrolledWindow.Ref.Child(Label.New("nil"));
                    
                if (schritt == 21)
                {
                    model = ListStore.New().AddWeakRef(() => WriteLine("model disposed"));
                    itemNameFactory = SignalListItemFactory
                        .New()
                        .Setup(OnListItemSetup)
                        .Bind(OnListItemBind)
                        .AddWeakRef(() => Console.WriteLine("itemNameFactory disposed"));
                    itemEMailFactory = SignalListItemFactory
                        .New()
                        .Setup(OnListItemSetup)
                        .Bind(OnEMailBind)
                        .AddWeakRef(() => Console.WriteLine("itemEMailFactory disposed"));
                    selectionModel = SingleSelection.New(model).AddWeakRef(() => WriteLine("SelectionModel disposed"));
                    scrolledWindow.Ref.Child(ColumnView
                            .New(selectionModel)
                            .AppendColumn(col1!)
                            .AppendColumn(col2!));
                    schritt = -1;
                }
                schritt++;
                GC.Collect();
                GC.Collect();
                PrintMemory();
            });
        }
    }

    static SignalListItemFactoryHandle itemNameFactory = new(0);
    static SignalListItemFactoryHandle itemEMailFactory = new(0);
    static SingleSelectionHandle selectionModel = new();

    static ColumnViewColumnHandle? col1;
    static ColumnViewColumnHandle? col2;

    static IListModel? model;

    static ObjectRef<ScrolledWindowHandle> scrolledWindow = new();

    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New());

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        label.Set($"{listItem.GetObject<Contact>()?.Name}");
    }

    static void OnEMailBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        label.Set($"{listItem.GetObject<Contact>()?.EMail}");
    }

    record Contact(string Name, string EMail, int Number);

    static void PrintMemory() => WriteLine($"Total memory: {Process.GetCurrentProcess().WorkingSet64:N0}, managed: {GC.GetTotalMemory(true):N0}");
}



