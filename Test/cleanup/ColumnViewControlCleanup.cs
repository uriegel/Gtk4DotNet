using System.Diagnostics;
using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.Controls;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

using static System.Console;
using static GtkDotNet.Controls.ColumnViewSubClassed;

static class ColumnViewControlCleanup
{
    // TODO ColumnView not finalized when changing model
    public static void Check()
    {
        var count = 1000_000;
        int schritt = 0;

        Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .SubClass(new ColumnViewControlCleanupClass())
                        .NewWindow()
                            .Title("Hello ColumnView Control👍")
                            .SideEffect(MemoryChecker)
                            .Titlebar(HeaderBar
                                .New()
                                .PackEnd(ToggleButton.New()
                                    .Label("Alternative Model")
                                    .BindProperty("active", changeItems, "sensitive", BindingFlags.InvertBoolean | BindingFlags.SyncCreate)
                                    .BindProperty("active", filterItems, "sensitive", BindingFlags.SyncCreate)
                                    .OnToggled(ModelToggled))
                                .PackEnd(ToggleButton.New()
                                    .Ref(changeItems)
                                    .Label("Change Items")
                                    .OnToggled(ChangeItems))
                                .PackEnd(ToggleButton.New()
                                    .Ref(filterItems)
                                    .Label("Filter")
                                    .OnToggled(FilterItems)))
                            .DefaultSize(800, 800)
                            .Child(ColumnViewControl.Create("ColumnView")
                                .SideEffect(cv =>
                                {
                                    columnView = cv.GetInstance() as ColumnViewControl;
                                    columnView?.SetController(controller1);
                                    controller1.Fill();
                                }))
                            .Show())
                .Run(0, IntPtr.Zero);

        void MemoryChecker(WindowHandle w)
        {
            w.SetTimer(300, TimeSpan.FromSeconds(5), () =>
            {
                if (controller2Active)
                    return;
                PrintMemory();
                var now = DateTime.Now;
                if (schritt % 2 == 0)
                    controller1.RemoveAll();
                if (schritt % 2 == 1)
                    controller1.FillMany(count);

                var stamp = DateTime.Now - now;
                WriteLine($"Duration: {stamp}");

                if (schritt > 0 && schritt % 2 == 0)
                    controller1.RemoveAll();

                schritt++;
                GC.Collect();
                GC.Collect();
                PrintMemory();
            });
        }
    }
    static void ModelToggled(ToggleButtonHandle toggleButton)
    {
        if (toggleButton.Active())
        {
            controller2Active = true;
            columnView?.SetController(controller2);
            controller2.Fill();
        }
        else
        {
            controller2Active = false;
            columnView?.SetController(controller1);
            controller1.Fill();
        }
    }

    static void ChangeItems(ToggleButtonHandle toggleButton)
        => controller1.Insert(2, [
            new Type1("New Item 1", 2001),
            new Type1("New Item 2", 2012),
            new Type1("New Item 3", 2023)]);

    static void FilterItems(ToggleButtonHandle toggleButton)
    {
        Controller2.IsFiltering = toggleButton.Active();
        columnView?.FilterChanged(Controller2.IsFiltering ? FilterChange.MoreStrict : FilterChange.LessStrict);
    }

    static readonly Controller1 controller1 = new();
    static readonly Controller2 controller2 = new();
    static readonly ObjectRef<ToggleButtonHandle> changeItems = new();
    static readonly ObjectRef<ToggleButtonHandle> filterItems = new();

    static ColumnViewControl? columnView;

    class ColumnViewControlCleanupClass()
        : ColumnViewSubClassedClass("ColumnView", p => new ColumnViewControl(p)) { }

    class Controller1 : Controller<Type1>
    {
        public Controller1()
        {
            MultiSelection = true;
            EnableRubberband = true;
        }

        public override Column<Type1>[] GetColumns()
            => [ new()
                    {
                        Title = "Name",
                        Expanded = true,
                        OnLabelBind = i => i.Name,
                        OnSort = (a, b, d) => string.Compare(a.Name, b.Name)
                    },
                new()
                    {
                        Title = "Number",
                        OnLabelBind = i => i.Number.ToString(),
                        OnSort = (a, b, d) => a.Number - b.Number
                    },
                ];

        public void Fill() => Insert([
                new Type1("Uwe Riegel", 1965),
                new Type1("James Bond", 1962),
                new Type1("Harry Henry", 1982),
                new Type1("Mike Michels", 1992),
                new Type1("Jim Doe", 222),
                new Type1("Jane Doe", 9999)]);
        public void FillMany(int count) => Insert(Enumerable.Range(0, count).Select(n => new Type1($"Item no {n}", n)));
    }

    static bool controller2Active;

    class Controller2 : Controller<Type2>
    {
        public static bool IsFiltering { get; set; }
        public Controller2() => OnFilter = Filter;
        public override Column<Type2>[] GetColumns()
            => [ new()
                    {
                        Title = "E Mail",
                        Expanded = true,
                        OnItemSetup = OnIconName,
                        OnItemBind = OnIconNameBind,
                        OnSort = (a, b, d) => string.Compare(a.EMail, b.EMail)
                    },
                new()
                    {
                        Title = "ID", OnLabelBind = i => i.Id
                    },
                new()
                    {
                        Title = "Active", OnLabelBind = i => i.Active ? "Yes" : "No",
                        OnSort = (a, b, d) => a.Active.CompareTo(b.Active)
                    },
                ];

        public void Fill() => Insert([
            .. Enumerable.Range(1, 100_000).Select(n => new Type2($"item{n}@dom.de", $"ID-{n}", n % 3 == 0))]);

        static bool Filter(Type2 item)
            => IsFiltering
                ? item.Active
                : true;

        static BoxHandle OnIconName()
                => Box
                    .New(Orientation.Horizontal)
                    .Append(Image.NewFromIconName("mail", IconSize.Button))
                    .Append(Label.New().HAlign(Align.Start).MarginStart(5));

        static void OnIconNameBind(ListItemHandle listItem, Type2 item)
        {
            var box = listItem.GetChild<BoxHandle>();
            var image = box?.GetFirstChild<ImageHandle>();
            var label = image?.GetNextSibling<LabelHandle>();
            if (item.Active)
                image?.SetFromIconName("mail-read", IconSize.LargeToolbar);
            else
                image?.SetFromIconName("mail-unread", IconSize.LargeToolbar);
            label?.Set(item.EMail);
        }
    }

    class ColumnViewControl(nint obj) : ColumnViewSubClassed(obj)
    {
        protected override void OnCreate() { }
        protected override void OnFinalize() => Console.WriteLine("ColumnView finalized");
        protected override CustomColumnViewHandle CreateHandle(nint obj) => new(obj);
    }

    record Type1(string Name, int Number);
    record Type2(string EMail, string Id, bool Active);    
    
    static void PrintMemory() => WriteLine($"Total memory: {Process.GetCurrentProcess().WorkingSet64:N0}, managed: {GC.GetTotalMemory(true):N0}");
}