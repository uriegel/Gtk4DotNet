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

            return compareFunc(obj1.GetManagedData<T>(ListStore.DATA), obj2.GetManagedData<T>(ListStore.DATA));
        }
        CompareDataDelegate compareDataDelegate = RawCompare;
        var res = New(compareDataDelegate, 0, 0);
        var key = GtkDelegates.Instance.GetKey("CustomSorter");
        GtkDelegates.Instance.Add(key, compareDataDelegate);
        res.AddWeakRef(() => GtkDelegates.Instance.Remove(key.Key));
        res.CheckDiagnostics();
        return res;
    }

    // public static void OnChanged(Action<bool, SorterChange> onChanged)
    //     => SignalConnect<OnChangedDelegate>(s, "changed", (col, sorterChanged, __) =>
    //     {
    //         bool desc = GetPrimaryOrder(col) != 0;
    //         onChanged(desc, sorterChanged);
    //     });


    [DllImport(Libs.LibGtk, EntryPoint = "gtk_custom_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomSorter New(CompareDataDelegate compare, nint nil, nint nil2);

    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_sorter_get_primary_sort_order", CallingConvention = CallingConvention.Cdecl)]
    // extern static int GetPrimaryOrder(nint col);
}

delegate int CompareDataDelegate(nint data1, nint data2, nint nil);
//delegate void OnChangedDelegate(nint _, SorterChange sorterChange, nint nil);