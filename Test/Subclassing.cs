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
        var input = ReadLine();
        switch (input)
        {
            case "1":
                RunGObject();
                break;
            case "2":
                RunButton();
                break;
        }
        return 0;
    }
    static void RunGObject()
    {
        var tDoubleClass = new TDoubleClass(Parent.GObject, "TDouble", p => new TDouble(p));

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
                   .SideEffect(_ => customButtonClass = new CustomButtonClass(Parent.Button, "CustomButton", p => new CustomButton(p)))
                   .NewWindow()
                       .Title("Hello Gtk👍")
                       .DefaultSize(600, 200)
                       .Child(
                            Box
                                .New(Orientation.Vertical)
                                .Append(customButtonClass!.New())
                                .Append(customButtonClass!.New())
                       )
                       .Show())
            .Run(0, IntPtr.Zero);

    static CustomButtonClass? customButtonClass;
}

// TODO CustomButton with label text
// TODO click counts and displays in label
// TODO Test Custom Box with Box with 2 CustomButtons from ui
// TODO Custom properties
// TODO gtk_combo_box_get_type
class TDoubleClass(Parent parent, string name, Func<nint, TDouble> constructor)
    : SubClass<GObjectHandle>(parent, name, constructor)
{
}

class TDouble(nint obj) : SubClassInst<GObjectHandle>(obj)
{
    protected override void OnCreate() => WriteLine("TDouble created");
    protected override void OnFinalize() => WriteLine("TDouble finalized");

    protected override GObjectHandle CreateHandle(nint obj) => new(obj);
}


class CustomButtonClass(Parent parent, string name, Func<nint, CustomButton> constructor) 
    : SubClass<ButtonHandle>(parent, name, constructor)
{
}

class CustomButton(nint obj) : SubClassInst<ButtonHandle>(obj)
{
    protected override void OnFinalize() => WriteLine("Button finalized");
    protected override ButtonHandle CreateHandle(nint obj) => new(obj);
}