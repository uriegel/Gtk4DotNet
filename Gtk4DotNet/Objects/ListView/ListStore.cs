using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ListStore : ListModel
{
    public static ListStore New()
    {
        var res = New(Type());
        res.CheckDiagnostics();
        res.AutoDestroyed = true;
        return res;
    }

    public void Append<T>(T t)
        where T : class
    {
        using var obj = NewObject();
        obj.SetManagedData(DATA, t);
        Append(this, obj);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListStore New(nint type);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(ListStore store, GObject obj);

    internal const string DATA = "DATA";
}
        