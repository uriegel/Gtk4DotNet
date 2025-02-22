using System.Buffers;
using System.Runtime.InteropServices;
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

    static void RunWidget()
        => Application
           .New("org.gtk.example")
           .OnActivate(app =>
               app
                   //                   .SideEffect(_ => customBoxClass = new CustomBoxClass(GTypeEnum.Box, "CustomBox", p => new CustomBox(p)))
                   .NewWindow()
                       .Title("Hello Gtk👍")
                       .DefaultSize(600, 200)
                       //                     .Child(customBoxClass!.New())
                       .Show())
            .Run(0, IntPtr.Zero);

}

// TODO new Example: load ui builder template with a CustomButton
// TODO Remove all templates in widgets
// TODO Downcast operator : widgetHandle to WindowHandle,  BoxHandle ... generic

// TODO Template initialization in Gtk4DotNet library not working!!!
// TODO Manually parsing ui template:
// TODO menu in AdwHeaderbar
// TODO custom widgets in ui template
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
        => Handle.OnClicked(() =>
        {
            Handle.Label($"{++count} times clicked");
            var instance = GetInstance(Handle);
        });
    protected override void OnFinalize() => WriteLine("Button finalized");
    protected override ButtonHandle CreateHandle(nint obj) => new(obj);

    int count;
}

// Custom Box ========================================================================================================================

class CustomBoxClass(GTypeEnum parent, string name, Func<nint, CustomBox> constructor)
    : SubClass<BoxHandle>(parent, name, constructor)
{
    // protected override void ClassInit(nint gClass, nint classData)
    // {
    //     base.ClassInit(gClass, classData);
    //     //InitTemplateFromResource(gClass, "custombox");
    // }
}

class CustomBox(nint obj) : SubClassInst<BoxHandle>(obj)
{
    protected override void OnCreate()
    {
        var bülder = Builder.FromDotNetResource("custombox");



        gtk_builder_get_objects(bülder.GetInternalHandle(), out var objekte);
        int count = objekte.ToInt32();
        IntPtr[] objects = new IntPtr[count];
        Marshal.Copy(objekte, objects, 0, count);


        var instance = bülder.GetWidget("instance");
        gtk_widget_set_parent(instance.GetInternalHandle(), 0);
        // Handle.InitTemplate();



        // var pointer = gtk_widget_get_template_child(Handle.GetInternalHandle(), CustomBoxClass.Klasse, "label");


        // var affe = gtk_widget_lookup(Handle.GetInternalHandle(), "label");

        var alls = Handle.GetAllChildren().Select(n => n.GetName()).ToArray();
    }

    protected override void OnFinalize() => WriteLine("Box finalized");
    protected override BoxHandle CreateHandle(nint obj) => new(obj);

    [DllImport("libgtk-4.so.1")]
    public static extern IntPtr gtk_builder_get_objects(IntPtr builder, out IntPtr n_objects);

    [DllImport("libgtk-4.so.1")]
    public static extern void gtk_widget_set_parent(IntPtr widget, IntPtr parent);
}