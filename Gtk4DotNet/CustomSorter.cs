using System.Runtime.InteropServices;
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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_custom_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomSorterHandle New(CompareDataDelegate compare, nint nil, nint nil2);
}

delegate int CompareDataDelegate(nint data1, nint data2, nint nil);