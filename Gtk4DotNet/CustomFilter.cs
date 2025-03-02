using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class CustomFilter
{
    public static CustomFilterHandle New<THandle>(Func<THandle, bool> predicate)
        where THandle : ObjectHandle, new()
    {
        bool RawCompare(nint d, nint _)
        {
            var data = new THandle();
            data.SetInternalHandle(d);
            data.IsFloating = true;
            return predicate(data);
        }
        CustomFilterDelegate customFilterDelegate = RawCompare;
        // TODO addweakref 
        GtkDelegates.Add(customFilterDelegate);
        return New(customFilterDelegate, 0, 0);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_changed", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Changed(this CustomFilterHandle filter, FilterChange filterChanged);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_custom_filter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomFilterHandle New(CustomFilterDelegate compare, nint nil, nint nil2);
}
// TODO free delegate: 
// TODO perhaps subclassing CustomSorter and freeing in finalizer

delegate bool CustomFilterDelegate(nint data, nint nil);