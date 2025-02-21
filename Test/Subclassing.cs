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
        d2.Dispose();
        d1.Dispose();
    }
}

// TODO return CustomType
// TODO create sub class with override functions
// TODO create Instances with functions (new and from ui)
// TODO Test 2 CustomButtons
// TODO Test 2 CustomButtons in ui
// TODO Custom properties
// TODO [DllImport("libgtk-4.so.1", EntryPoint = "g_object_new", CallingConvention = CallingConvention.Cdecl)]
// TODO g_object_new FloatingHandle
// TODO g_object_new ObjectHandle to release
// TODO Constructor
// TODO FInalizer


class TDoubleClass(Parent parent, string name, Func<nint, TDouble> constructor) 
    : SubClass<GObjectHandle>(parent, name, constructor)
{

 //   WriteLine("TDoubleClass ctor");
}


class TDouble(nint obj) : SubClassInst<GObjectHandle>(obj)
{
   // WriteLine("TDoubleClass ctor");

    protected override GObjectHandle CreateHandle(nint obj) => new GObjectHandle(obj);
            
}