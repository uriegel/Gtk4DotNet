using System.Runtime.InteropServices;
using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

using static System.Console;

static class SubClassing
{
    public static int Run()
    {
        WriteLine("1 - GObject");
        WriteLine("2 - Custom Buttom");
        WriteLine("3 - Custom Buttom in template");
        WriteLine("4 - Custom Window");

        var input = ReadLine();
        switch (input)
        {
            case "1":
                RunGObject();
                break;
            case "2":
                RunButton();
                break;
            case "3":
                RunBuilder();
                break;
            case "4":
                RunCustomWindow();
                break;
        }
        return 0;
    }
    static void RunGObject()
    {
        var typeDouble = "TDouble".TypeFromName();
        var tDoubleClass = new TDoubleClass(GTypeEnum.GObject, "TDouble", p => new TDouble(p));
        var customButtonClass = new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p));
        typeDouble = "TDouble".TypeFromName();
        var typeCustomButton = "CustomButton".TypeFromName();

        var obj = GObject.New<GObjectHandle>(typeDouble);
        var obj2 = GObject.New<GObjectHandle>(typeDouble);
        var refcount = Marshal.ReadInt32(obj.GetInternalHandle(), IntPtr.Size);
        obj2.Dispose();
        obj.Dispose();
        refcount = Marshal.ReadInt32(obj.GetInternalHandle(), IntPtr.Size);
    }

    static void RunButton()
        => Application
           .New("org.gtk.example")
           .OnActivate(app =>
               app
                   .SubClass(new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                   .SubClass(new TDoubleClass(GTypeEnum.Button, "TDouble", p => new TDouble(p)))
                   .NewWindow()
                       .Title("Hello Gtk👍")
                       .DefaultSize(600, 200)
                       .Child(
                            Box
                                .New(Orientation.Vertical)
                                .Append(GObject.New<ButtonHandle>("CustomButton".TypeFromName())
                                    .Label("Button 1"))
                                .Append(GObject.New<ButtonHandle>("CustomButton".TypeFromName())
                                    .Label("Button 2"))
                       )
                       .Show())
            .Run(0, IntPtr.Zero);

    public static int RunBuilder()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => app
                .SubClass(new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                .SideEffect(app =>
                    Builder.FromDotNetResource("buildersubclass").Use(
                        builder => builder
                            .GetObject<WindowHandle>("window", w => w
                                .SetApplication(app)
                                .SideEffect(w => 
                                    builder
                                        .SideEffect(b => b.GetObject<ButtonHandle>("button1", b => b
                                            .OnClicked(() => WriteLine("Button1 clicked"))))
                                        .SideEffect(b => b.GetObject<ButtonHandle>("button2", b => b
                                            .OnClicked(() => WriteLine("Button2 clicked"))))
                                        .SideEffect(b => b.GetObject<ButtonHandle>("quit", b => b
                                            .OnClicked(() => w.CloseWindow()))))
                                .Show()))))
            .Run(0, IntPtr.Zero);

    static void RunCustomWindow()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => app
                .SubClass(new CustomWindowClass(GTypeEnum.Window, "CustomWindow", p => new CustomWindow(p)))
                .SubClass(new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                .SideEffect(a =>
                    GObject.New<WindowHandle>("CustomWindow".TypeFromName())
                        .SetApplication(app)
                        .Show()))
            .Run(0, IntPtr.Zero);
}

// TODO parallel to window a menu
// TODO Connect actions
// TODO Access menu items

// TODO Remove all templates in widgets
// TODO Downcast operator : widgetHandle to WindowHandle,  BoxHandle ... generic

// TODO menu in AdwHeaderbar
// TODO custom widgets in ui template
// TODO Custom properties
// TODO gtk_combo_box_get_type

// Custom GObject ========================================================================================================================
class TDoubleClass(GTypeEnum parent, string name, Func<nint, TDouble> constructor)
    : SubClass<GObjectHandle>(parent, name, constructor) { }

class TDouble(nint obj) : SubClassInst<GObjectHandle>(obj)
{
    protected override void OnCreate() => WriteLine("TDouble created");
    protected override void OnFinalize() => WriteLine("TDouble finalized");

    protected override GObjectHandle CreateHandle(nint obj) => new(obj);
}

// Custom Button ========================================================================================================================

class CustomButtonClass(GTypeEnum parent, string name, Func<nint, CustomButton> constructor)
    : SubClass<ButtonHandle>(parent, name, constructor) {}

class CustomButton(nint obj) : SubClassInst<ButtonHandle>(obj)
{
    protected override void OnCreate() => Handle.OnClicked(() => Handle.Label($"{++count} times clicked"));

    protected override void OnFinalize() => WriteLine("Button finalized");
    protected override ButtonHandle CreateHandle(nint obj) => new(obj);

    int count;
}

// Custom Window ========================================================================================================================

class CustomWindowClass(GTypeEnum parent, string name, Func<nint, CustomWindow> constructor)
    : SubClass<WindowHandle>(parent, name, constructor)
{
    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, "windowsubclass");
    }
}

class CustomWindow(nint obj) : SubClassInst<WindowHandle>(obj)
{
    protected override void OnCreate()
    {
        Handle.InitTemplate();
        Handle
            .GetTemplateChild<ButtonHandle, WindowHandle>("button1")
            ?.OnClicked(() => WriteLine("Button1 clicked"));
        Handle
            .GetTemplateChild<ButtonHandle, WindowHandle>("quit")
            ?.OnClicked(() => Handle.CloseWindow());
    }
    protected override void OnFinalize() => WriteLine("Window finalized");
    protected override WindowHandle CreateHandle(nint obj) => new(obj);
}

