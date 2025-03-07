using GtkDotNet;

static class CustomColumnView
{
    public static int Run()
        => Application
            .NewAdwaita("de.uriegel.first")
                .OnActivate(app =>
                    app
                        .SubClass(ManagedApplicationWindowClass.Register(p => new AppWindow(p), "customcolumnview"))
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
