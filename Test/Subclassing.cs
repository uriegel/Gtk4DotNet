using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

using static System.Console;

static class SubClassing
{
    public static int Run()
    {
        WriteLine("1 - GObject");
        var input = ReadLine();
        switch (input)
        {
            case "1":
                RunGObject();
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

        // Application
        //     .New("org.gtk.example")
        //     .OnActivate(app =>
        //         app
        //             .NewWindow()
        //                 .Title("Hello Gtk👍")
        //                 .DefaultSize(600, 200)
        //                 .Child(new TDoubleClass(Parent.Button, "CustomButton", p => new TDouble(p)).New().Handle)
        //                 .Show())
        //     .Run(0, IntPtr.Zero);
    }
}

// [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_type", CallingConvention = CallingConvention.Cdecl)]
// public static extern GTypeHandle Type();        

// public ButtonHandle(nint obj) : base() => SetInternalHandle(obj);

// public enum Parent

// GTypeHandle InitializeParentType()

// TODO create Instances with functions (new and from ui)
// TODO Test 2 CustomButtons
// TODO Test 2 CustomButtons in ui
// TODO Custom properties

class TDoubleClass(Parent parent, string name, Func<nint, TDouble> constructor)
    : SubClass<GObjectHandle>(parent, name, constructor)
{
}

class TDouble(nint obj) : SubClassInst<GObjectHandle>(obj)
{
    protected override void OnCreate() => WriteLine("TDouble created");
    protected override void OnFinalize() => WriteLine("TDouble finalized");

    protected override GObjectHandle CreateHandle(nint obj) => new GObjectHandle(obj);
}


// class TDoubleClass(Parent parent, string name, Func<nint, TDouble> constructor) 
//     : SubClass<ButtonHandle>(parent, name, constructor)
// {

//  //   WriteLine("TDoubleClass ctor");
// }


// class TDouble(nint obj) : SubClassInst<ButtonHandle>(obj)
// {
//    // WriteLine("TDoubleClass ctor");

//     protected override ButtonHandle CreateHandle(nint obj) => new ButtonHandle(obj);
            
// }