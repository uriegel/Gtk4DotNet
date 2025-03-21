using GtkDotNet;
using GtkDotNet.Controls;
using GtkDotNet.SafeHandles;

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
    {
        Handle.InitTemplate();
        var customColumnViewHandle = Handle.GetTemplateChild<CustomColumnViewHandle, ApplicationWindowHandle>("columnview");
        var columnView = customColumnViewHandle != null ? CustomColumnView.GetInstance(customColumnViewHandle) : null;
        columnView?.Fill();
    }
        

    protected override void OnFinalize() => Console.WriteLine("Window finalized");
}

class CustomColumnViewClass()
    : ColumnViewSubClassedClass("ColumnView", p => new CustomColumnView(p)) { }

class CustomColumnView(nint obj) : ColumnViewSubClassed(obj)
{
    public static CustomColumnView? GetInstance(CustomColumnViewHandle handle)
        => GetInstance(handle.GetInternalHandle()) as CustomColumnView;
            
    public void Fill() => controller.Fill();

    protected override void OnCreate()
    {
        MultiSelection = true;
        columnView?.AddController(GestureClick.New().OnPressed((i, d, b) =>
        {
            var display = columnView.GetDisplay();
            var display2 = Display.GetDefault();
            var seat = display.GetDefaultSeat();
            var device = seat.GetKeyboard();
            var status = device.GetModifierState();

            Console.WriteLine($"Mouse button pressed: {status}");
        })).AddController(GestureClick.New().OnReleased((i, d, b) =>
        {
            Console.WriteLine($"Mouse button released");
        }));
        SetController(controller);
    } 

    protected override void OnFinalize() => Console.WriteLine("ColumnView finalized");
    protected override CustomColumnViewHandle CreateHandle(nint obj) => new(obj);

    static readonly Controller2 controller = new()
    {
        EnableRubberband = true
    };
}
