using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class CustomSorter
{
    public static CustomSorterHandle New(Func<nint, nint, int> compareFunc)
    {
        int RawCompare(nint d1, nint d2, nint _)
        {
            return compareFunc(d1, d2);
        }
        CompareDataDelegate compareDataDelegate = RawCompare;
        var key = GtkDelegates.Add(compareDataDelegate);
        var res = New(compareDataDelegate, 0, 0);
        res.AddWeakRefRaw(() => GtkDelegates.Remove(key));
        return res;
    }

    public static CustomSorterHandle OnChanged(this CustomSorterHandle sorter, Action<bool, SorterChange> onChanged)
        => sorter.SideEffect(s => Gtk.SignalConnect<OnChangedDelegate>(s, "changed", (col, sorterChanged, __) =>
        {
            bool desc = GetPrimaryOrder(col) != 0;
            onChanged(desc, sorterChanged);
        }));


    [DllImport(Libs.LibGtk, EntryPoint = "gtk_custom_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomSorterHandle New(CompareDataDelegate compare, nint nil, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_sorter_get_primary_sort_order", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetPrimaryOrder(nint col);
}

delegate int CompareDataDelegate(nint data1, nint data2, nint nil);
delegate void OnChangedDelegate(nint _, SorterChange sorterChange, nint nil);