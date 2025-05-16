using System.Diagnostics;
using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.SafeHandles;

using static System.Console;

static class StringListViewCleanup
{
    public static void Check()
    {
        var count = 1_000_000;

        var model = StringList.New([]);
        var itemFactory = SignalListItemFactory
            .New();
        itemFactory.Setup(OnListItemSetup);
        itemFactory.Bind(OnListItemBind);
        itemFactory.AddWeakRef(() => WriteLine("Factory disposed"));
        var selectionModel = SingleSelection.New(model).AddWeakRef(() => WriteLine("SelectionModel disposed"));

        int schritt = 0;

        Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .SideEffect(MemoryChecker)
                    .Title("Hello Gtk Check👍")
                    .DefaultSize(400, 600)
                    .Child(ScrolledWindow
                        .New()
                        .Ref(scrolledWindow)
                        .Policy(PolicyType.Never, PolicyType.Automatic)
                        .Child(ListView
                            .New(selectionModel, itemFactory).AddWeakRef(() => WriteLine("ListView disposed"))))
                    .Show())
            .Run(0, IntPtr.Zero);

        static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New());

        static void OnListItemBind(ListItemHandle listItem)
        {
            var label = listItem.GetChild<LabelHandle>();
            var item = listItem.GetStringItem();
            label.Set(item);
        }

        void MemoryChecker(WindowHandle w)
        {
            w.SetTimer(300, TimeSpan.FromSeconds(5), () =>
            {
                PrintMemory();
                var now = DateTime.Now;
                if (schritt != 20 && schritt != 21 && schritt % 2 == 1)
                    model.Splice(0, 0, Enumerable
                               .Range(1, count)
                               .Select(n => $"Item no {n}"));

                var stamp = DateTime.Now - now;
                WriteLine($"Duration: {stamp}");

                if (schritt != 20 && schritt != 21 && schritt > 0 && schritt % 2 == 0)
                    model?.Splice(0, count);

                if (schritt == 20)
                    scrolledWindow.Ref.Child(Label.New("nil"));
                if (schritt == 21)
                {
                    model = StringList.New([]);
                    itemFactory = SignalListItemFactory
                        .New();
                    itemFactory.Setup(OnListItemSetup);
                    itemFactory.Bind(OnListItemBind);
                    itemFactory.AddWeakRef(() => WriteLine("Factory disposed"));
                    selectionModel = SingleSelection.New(model).AddWeakRef(() => WriteLine("SelectionModel disposed"));
                    scrolledWindow.Ref.Child(ListView
                            .New(selectionModel, itemFactory).AddWeakRef(() => WriteLine("ListView disposed")));
                    schritt = -1;
                }
                schritt++;
                GC.Collect();
                GC.Collect();
                PrintMemory();
            });
        }
    }

    static ObjectRef<ScrolledWindowHandle> scrolledWindow = new();

    static void PrintMemory() => WriteLine($"Total memory: {Process.GetCurrentProcess().WorkingSet64:N0}, managed: {GC.GetTotalMemory(true):N0}");
}