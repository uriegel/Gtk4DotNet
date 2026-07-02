using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class CustomSorter : Sorter
{
    public static CustomSorter New<T>(Func<T?, T?, int> compareFunc)
    {
        int RawCompare(nint p1, nint p2, nint _)
        {
            var obj1 = new GObject
            {
                AutoDestroyed = true
            };
            obj1.SetInternalHandle(p1);
            var obj2 = new GObject
            {
                AutoDestroyed = true
            };
            obj2.SetInternalHandle(p2);

            return compareFunc(obj1.GetManagedData<T>(Quark.ListData), obj2.GetManagedData<T>(Quark.ListData));
        }
        CompareDataDelegate compareDataDelegate = RawCompare;
        var res = New(compareDataDelegate, 0, 0);
        var key = GtkDelegates.Instance.GetKey("CustomSorter");
        GtkDelegates.Instance.Add(key, compareDataDelegate);
        res.AddWeakRef(() => GtkDelegates.Instance.Remove(key.Key));
        res.CheckDiagnostics();
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_custom_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomSorter New(CompareDataDelegate compare, nint nil, nint nil2);
}

delegate int CompareDataDelegate(nint data1, nint data2, nint nil);



