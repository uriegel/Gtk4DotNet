
using GtkDotNet;
using GtkDotNet.Controls;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

static class Dialogs
{
    public static int Run()
        => Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                .SubClass(ManagedAdwApplicationWindowClass.Register(p => new AdwMainWindow(p), "adwwindowsubclass"))
                .ManagedAdwApplicationWindow()
                .Show())
                .Run(0, IntPtr.Zero);
}

class AdwMainWindow(nint obj) : ManagedAdwApplicationWindow(obj)
{
    public static void Register(ApplicationHandle app)
        => app.SubClass(new AdwMainWindowClass());

    protected override void OnCreate()
    {
        Handle.InitTemplate();
    }

    protected override void Initialize()
    {
        var button = Handle.GetTemplateChild<ButtonHandle, ApplicationWindowHandle>("button1");
        button?.OnClicked(() =>
        {
            var dialog = AdwAlertDialog.New("Heading", "Der Körper");
            dialog.Present(Handle);
        });
    }

    public class AdwMainWindowClass()
        : SubClass<AdwApplicationWindowHandle>(GTypeEnum.ApplicationWindow, "mainwindow", p => new AdwMainWindow(p))
    { }

    protected override void OnFinalize() => Console.WriteLine("Window finalized");
    protected override AdwApplicationWindowHandle CreateHandle(nint obj) => new(obj);
}

