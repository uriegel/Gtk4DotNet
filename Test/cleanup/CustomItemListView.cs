using System.Diagnostics;
using System.Runtime.InteropServices;
using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

using static System.Console;

static class CustomItemListViewCleanup
{
    public static void Check()
    {
        var count = 1000_000;
        void InitStore(ApplicationHandle _)
        {
            model = ListStore.New(GObject.Type()).AddWeakRef(() => WriteLine("model disposed"));
            itemFactory = SignalListItemFactory
                .New()
                .Setup(OnListItemSetup)
                .Bind(OnListItemBind)
                .AddWeakRef(() => WriteLine("Factory disposed"));
            selectionModel = SingleSelection.New(model).AddWeakRef(() => WriteLine("SelectionModel disposed"));
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
                        .Child(ListView
                            .New(selectionModel, itemFactory)))
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
                WriteLine($"Dauerte: {stamp}");

                if (schritt != 20 && schritt != 21 && schritt > 0 && schritt % 2 == 0)
                    model?.RemoveItems(0, (uint)count);

                if (schritt == 20)
                    scrolledWindow.Ref.Child(Label.New("nil"));
                if (schritt == 21)
                {
                    model = ListStore.New().AddWeakRef(() => WriteLine("model disposed"));
                    itemFactory = SignalListItemFactory
                        .New()
                        .Setup(OnListItemSetup)
                        .Bind(OnListItemBind)
                        .AddWeakRef(() => WriteLine("Factory disposed"));
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

    static SignalListItemFactoryHandle itemFactory = new(0);
    static SingleSelectionHandle selectionModel = new();

    static IListModel? model;

    static ObjectRef<ScrolledWindowHandle> scrolledWindow = new();

    static void OnListItemSetup(ListItemHandle listItem) => listItem.SetChild(Label.New(""));

    static void OnListItemBind(ListItemHandle listItem)
    {
        var label = listItem.GetChild<LabelHandle>();
        label.Set($"{listItem.GetObject2<Contact>()?.Name}");
    }

    record Contact(string Name, string EMail, int Number);

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

        protected override void OnFinalize() => WriteLine("Contact finalized");
    }

    class TDoubleClass(GTypeEnum parent, string name, Func<nint, TDouble> constructor)
        : SubClass<GObjectHandle>(parent, name, constructor) { }

    class TDouble(nint obj) : SubClassInst<GObjectHandle>(obj)
    {
        public float Value { get; set; }
        protected override void OnCreate() => WriteLine("TDouble created");
        protected override void OnFinalize() => WriteLine("TDouble finalized");

        protected override GObjectHandle CreateHandle(nint obj) => new(obj);
    }

    static void PrintMemory() => WriteLine($"Total memory: {Process.GetCurrentProcess().WorkingSet64:N0}, managed: {GC.GetTotalMemory(true):N0}");
}



