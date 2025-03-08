using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

static class CustomColumnViewApp
{
    public static int Run()
        => Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .SubClass(ManagedApplicationWindowClass.Register(p => new AppWindow(p), "customcolumnview"))
                        .SubClass(new CustomColumnViewClass())
                        .ManagedApplicationWindow()
                        .Show())
                .Run(0, IntPtr.Zero);
}

class AppWindow(nint obj) : ManagedApplicationWindow(obj)
{
    protected override void OnCreate()
        => Handle.InitTemplate();

    protected override void OnFinalize() => Console.WriteLine("Window finalized");
}

class CustomColumnViewClass()
    : ColumnViewSubClassedClass("ColumnView", p => new CustomColumnView(p)) { }

class CustomColumnView(nint obj) : ColumnViewSubClassed(obj)
{
    protected override void OnCreate()
    {
        SetColumns(GetColumns());
    }
    protected override void OnFinalize() => Console.WriteLine("ColumnView finalized");
    protected override CustomColumnViewHandle CreateHandle(nint obj) => new(obj);
    
    static Column<Type1>[] GetColumns()
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
}
