using System.Runtime.InteropServices;
using GtkDotNet;
using LinqTools;
using static System.Console;

static class Cleanup
{
    public static int Run()
        => Application
            .New("de.urigel.test")
            .OnActivate(app =>
            {
                var label = New("label");
                AddWeakRef(label, (a, b) =>
                {
                    var affe = 0;
                }, IntPtr.Zero);
                //Unref(label);
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

static class Libs
{
    public const string LibGtk = "libgtk-4.so";

    public const string LibGio = "libgio-2.0.so";

    public const string LibWebKit = "libwebkitgtk-6.0.so";
}
