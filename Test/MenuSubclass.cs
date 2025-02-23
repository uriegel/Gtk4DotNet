using GtkDotNet;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;
using GtkDotNet.SubClassing;

using static System.Console;

static class MenuSubclass
{
    public static int Run()
        => Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .SubClass(new MenuWindowClass(GTypeEnum.Window, "MenuWindow", p => new MenuWindow(p)))
                    .SideEffect(a =>
                        GObject.New<WindowHandle>("MenuWindow".TypeFromName())
                        .SetApplication(app)
                        .Show()))                    
            .AddActions(
            [
                new GtkAction("quit", () => WriteLine("Quitting..."), "F4")
            ])
            .Run(0, IntPtr.Zero);
}

class MenuWindowClass(GTypeEnum parent, string name, Func<nint, MenuWindow> constructor)
    : SubClass<WindowHandle>(parent, name, constructor)
{
    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, "menu");
    }
}

class MenuWindow(nint obj) : SubClassInst<WindowHandle>(obj)
{
    protected override void OnCreate() => Handle.InitTemplate();
    protected override void OnFinalize() => WriteLine("Window finalized");
    protected override WindowHandle CreateHandle(nint obj) => new(obj);
}

