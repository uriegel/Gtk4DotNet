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
        Handle
            .GetTemplateChild<ButtonHandle, ApplicationWindowHandle>("alert-button")
            ?.OnClicked(async () =>
            {
                var dialog = Builder.FromDotNetResource("alertdialogbuilder").GetWidget<AdwAlertDialogHandle>("dialog");
                var response = await dialog.PresentAsync(Handle);
                var label = Handle.GetTemplateChild<LabelHandle, ApplicationWindowHandle>("label");
                label?.Set(response);
            });
        Handle
            .GetTemplateChild<ButtonHandle, ApplicationWindowHandle>("dialog-button")
            ?.OnClicked(() =>
            {
                var dialog = Builder.FromDotNetResource("dialogbuilder").GetWidget<AdwAlertDialogHandle>("dialog");
                dialog.Present(Handle);
            });
    }

    public class AdwMainWindowClass()
        : SubClass<AdwApplicationWindowHandle>(GTypeEnum.ApplicationWindow, "mainwindow", p => new AdwMainWindow(p))
    { }

    protected override void OnFinalize() => Console.WriteLine("Window finalized");
    protected override AdwApplicationWindowHandle CreateHandle(nint obj) => new(obj);
}

