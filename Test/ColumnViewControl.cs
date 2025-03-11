using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;
using static GtkDotNet.SubClassing.ColumnViewSubClassed;

static class ColumnViewControlApp
{
    public static int Run()
        => Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .SubClass(new ColumnViewControlClass())
                        .NewWindow()
                            .Title("Hello ColumnView Control👍")
                            .Titlebar(HeaderBar
                                .New()
                                .PackEnd(ToggleButton.New()
                                    .Label("Alternative Model")
                                    .BindProperty("active", changeItems, "sensitive", BindingFlags.InvertBoolean | BindingFlags.SyncCreate)
                                    .OnToggled(ModelToggled))
                                .PackEnd(ToggleButton.New()
                                    .Ref(changeItems)
                                    .Label("Change Items")
                                    .OnToggled(ChangeItems)))
                            .DefaultSize(600, 800)
                            .Child(ColumnViewControl.Create("ColumnView")
                                .SideEffect(cv =>
                                {
                                    columnView = cv.GetInstance() as ColumnViewControl;
                                    columnView?.SetController(controller1);
                                    controller1.Fill();
                                }))
                            .Show())
                .Run(0, IntPtr.Zero);

    static void ModelToggled(ToggleButtonHandle toggleButton)
    {
        // if (toggleButton.Active())
        //     columnView?.SetColumns(ColumnViewControl.GetColumns2()).Insert(ColumnViewControl.GetItems2());
        // else
        //     columnView?.SetColumns(ColumnViewControl.GetColumns1())
        //         .SideEffect(m => ColumnViewControl.model1 = m)
        //         .Insert(ColumnViewControl.GetItems1());
    }

    static void ChangeItems(ToggleButtonHandle toggleButton)
    {
        
    }
    // => ColumnViewControl.model1?.Insert(2, [
    //         new Type1("New Item 1", 2001),
    //         new Type1("New Item 2", 2012),
    //         new Type1("New Item 3", 2023)]);


    static readonly Controller1 controller1 = new();
    static readonly Controller2 controller2 = new();
    static readonly ObjectRef<ToggleButtonHandle> changeItems = new();

    static ColumnViewControl? columnView;
}

class ColumnViewControlClass()
    : ColumnViewSubClassedClass("ColumnView", p => new ColumnViewControl(p)) { }

class Controller1 : Controller<Type1>
{
    public override Column<Type1>[] GetColumns()
        => [ new()
                {
                    Title = "Name",
                    Expanded = true,
                    OnLabelBind = i => i.Name,
                    OnSort = (a, b) => string.Compare(a.Name, b.Name)
                },
            new()
                {
                    Title = "Number",
                    OnLabelBind = i => i.Number.ToString(),
                    OnSort = (a, b) => a.Number - b.Number
                },
            ];

    public void Fill() => Insert([
            new Type1("Uwe Riegel", 1965),
            new Type1("James Bond", 1962),
            new Type1("Harry Henry", 1982),
            new Type1("Mike Michels", 1992),
            new Type1("Jim Doe", 222),
            new Type1("Jane Doe", 9999)]);
}

class Controller2 : Controller<Type2>
{
    public override Column<Type2>[] GetColumns()
        => [ new()
                {
                    Title = "E Mail",
                    Expanded = true,
                    OnItemSetup = OnIconName,
                    OnItemBind = OnIconNameBind,
                    OnSort = (a, b) => string.Compare(a.EMail, b.EMail)
                },
            new()
                {
                    Title = "ID", OnLabelBind = i => i.Id
                },
            new()
                {
                    Title = "Active", OnLabelBind = i => i.Active ? "Yes" : "No",
                    OnSort = (a, b) => a.Active.CompareTo(b.Active)
                },
            ];

    static BoxHandle OnIconName()
        => Box
            .New(Orientation.Horizontal)
            .Append(Image.NewFromIconName("mail", IconSize.Button))
            .Append(Label.New("").HAlign(Align.Start).MarginStart(5));

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
    // TODO create 2 controller classes with columns and items


    public static IEnumerable<Type2> GetItems2()
        => [.. Enumerable.Range(1, 100_000).Select(n => new Type2($"item{n}@dom.de", $"ID-{n}", n % 3 == 0))];

    protected override void OnCreate()
    {
        MultiSelection = true;
        SetController(new Controller1());
        //model1.Insert(GetItems1());
    }
    protected override void OnFinalize() => Console.WriteLine("ColumnView finalized");
    protected override CustomColumnViewHandle CreateHandle(nint obj) => new(obj);
}

record Type1(string Name, int Number);
record Type2(string EMail, string Id, bool Active);