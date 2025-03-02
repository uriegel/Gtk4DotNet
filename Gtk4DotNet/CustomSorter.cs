using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class CustomSorter
{
    public static CustomSorterHandle New<THandle>(Func<THandle, THandle, int> compareFunc)
        where THandle : ObjectHandle, new()
    {
        int RawCompare(nint d1, nint d2, nint _)
        {
            var data1 = new THandle();
            data1.SetInternalHandle(d1);
            data1.IsFloating = true;
            var data2 = new THandle();
            data2.SetInternalHandle(d2);
            data2.IsFloating = true;
            return compareFunc(data1, data2);
        }
        // TODO addweakref
        CompareDataDelegate compareDataDelegate = RawCompare;
        GtkDelegates.Add(compareDataDelegate);
        return New(compareDataDelegate, 0, 0);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_custom_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomSorterHandle New(CompareDataDelegate compare, nint nil, nint nil2);
}
// TODO free delegate: 
// TODO perhaps subclassing CustomSorter and freeing in finalizer

delegate int CompareDataDelegate(nint data1, nint data2, nint nil);