using System.Runtime.InteropServices;
using GtkDotNet;
using LinqTools;

using static System.Console;

static class Cleanup
{
    public static int Run()
    {
        Test(Check1, "Finished 1");
        Test(Check2, "Finished 2");
        Test(Check3, "Finished 3");
        return 0;
    }

    static void Test(Action action, string text)
    {
        action();
        GC.Collect();
        GC.Collect();
        Thread.Sleep(1000);
        GC.Collect();
        GC.Collect();
        WriteLine(text);
        ReadLine();
    }

    static void Check1()
    {
        var test1 = new Test1();
    }

    static void Check2()
        => Application
            .New("de.urigel.test")
            .OnActivate(app =>
            {
                var test1 = new Test1();
    //             var label = New("label");
    //             AddWeakRef(label, (a, b) =>
    //             {
    //                 var affe = 0;
    //             }, IntPtr.Zero);
    //             //Unref(label);
            })
            .Run(0, IntPtr.Zero);

    static void Check3()
        => Application
            .New("de.urigel.test")
            .AddWeakRef(() => WriteLine("Application Check3 disposed"))
            .OnActivate(app => {
                var test1 = new Test1();
            })
            .Run(0, IntPtr.Zero);

    [DllImport(Libs.LibGtk, EntryPoint="g_free", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Free(this IntPtr obj);

    [DllImport(Libs.LibGtk, EntryPoint="g_object_unref", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unref(IntPtr obj);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr New(string text);

    [DllImport(Libs.LibGtk, EntryPoint="g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddWeakRef(IntPtr obj, FinalizerDelegate finalizer, IntPtr zero);

    delegate void FinalizerDelegate(IntPtr zero, IntPtr obj);
}

class Test1
{
   string test = "Hallo";

    ~Test1()
        => WriteLine("Test1 disposed");
}

static class Libs
{
    public const string LibGtk = "libgtk-4.so";

    public const string LibGio = "libgio-2.0.so";

    public const string LibWebKit = "libwebkitgtk-6.0.so";
}
