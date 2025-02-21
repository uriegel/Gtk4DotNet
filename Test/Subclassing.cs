using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using CsTools.Extensions;
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
        WriteLine("3 - Custom Box");

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
                RunWidget();
                break;
        }
        return 0;
    }
    static void RunGObject()
    {
        var tDoubleClass = new TDoubleClass(GTypeEnum.GObject, "TDouble", p => new TDouble(p));

        var d1 = tDoubleClass.New();
        var d2 = tDoubleClass.New();

        var refcount2 = Marshal.ReadInt32(d2.Handle.GetInternalHandle(), IntPtr.Size);
        d2.Dispose();
        refcount2 = Marshal.ReadInt32(d2.Handle.GetInternalHandle(), IntPtr.Size);
        d1.Dispose();
    }

    static void RunButton()
        => Application
           .New("org.gtk.example")
           .OnActivate(app =>
               app
                   .SideEffect(_ => customButtonClass = new CustomButtonClass(GTypeEnum.Button, "CustomButton", p => new CustomButton(p)))
                   .NewWindow()
                       .Title("Hello Gtk👍")
                       .DefaultSize(600, 200)
                       .Child(
                            Box
                                .New(Orientation.Vertical)
                                .Append(customButtonClass!.New()
                                    .Handle.Label("Button 1"))
                                .Append(customButtonClass!.New()
                                    .Handle.Label("Button 2"))
                       )
                       .Show())
            .Run(0, IntPtr.Zero);

    static void RunWidget()
        => Application
           .New("org.gtk.example")
           .OnActivate(app =>
               app
                   .SideEffect(_ => customBoxClass = new CustomBoxClass(GTypeEnum.Box, "CustomBox", p => new CustomBox(p)))
                   .NewWindow()
                       .Title("Hello Gtk👍")
                       .DefaultSize(600, 200)
                       .Child(customBoxClass!.New())
                       .Show())
            .Run(0, IntPtr.Zero);

    static CustomButtonClass? customButtonClass;
    static CustomBoxClass? customBoxClass;
}

// TODO Template initialization in Gtk4DotNet library not working!!!
// TODO Custom properties
// TODO gtk_combo_box_get_type

// Custom GObject ========================================================================================================================
class TDoubleClass(GTypeEnum parent, string name, Func<nint, TDouble> constructor)
    : SubClass<GObjectHandle>(parent, name, constructor)
{
}

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
    protected override void OnCreate()
        => Handle.OnClicked(() => Handle.Label($"{++count} times clicked"));
    protected override void OnFinalize() => WriteLine("Button finalized");
    protected override ButtonHandle CreateHandle(nint obj) => new(obj);

    int count;
}

// Custom Box ========================================================================================================================

class CustomBoxClass(GTypeEnum parent, string name, Func<nint, CustomBox> constructor)
    : SubClass<BoxHandle>(parent, name, constructor)
{
    protected override void ClassInit(nint gClass, nint classData)
    {
        base.ClassInit(gClass, classData);
        InitTemplateFromResource(gClass, "custombox");
    }
}

class CustomBox(nint obj) : SubClassInst<BoxHandle>(obj)
{
    protected override void OnCreate()
    {
        Handle.InitTemplate();
        var label = Handle.GetTemplateLabelChild(GTypeEnum.Widget, "label");
    }
        
    protected override void OnFinalize() => WriteLine("Box finalized");
    protected override BoxHandle CreateHandle(nint obj) => new(obj);
}