using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class CustomFilter : Filter
{
    public static CustomFilter New<T>(Func<T?, bool> predicate)
    {
        bool RawCompare(nint ptr, nint _)
        {
            var obj = new GObject
            {
                WeakCopy = true
            };
            obj.SetInternalHandle(ptr);
            return predicate(obj.GetManagedData<T>(ListStore.DATA));
        }
        ;
        CustomFilterDelegate customFilterDelegate = RawCompare;
        var res = New(customFilterDelegate, 0, 0);
        var key = GtkDelegates.Instance.GetKey("CustomFilter");
        GtkDelegates.Instance.Add(key, customFilterDelegate);
        res.AddWeakRef(() => GtkDelegates.Instance.Remove(key.Key));
        res.CheckDiagnostics();
        return res;
    }
    
    public void Changed(FilterChange filterChanged) => Changed(this, filterChanged);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_changed", CallingConvention = CallingConvention.Cdecl)]
    extern static void Changed(CustomFilter filter, FilterChange filterChanged);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_custom_filter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomFilter New(CustomFilterDelegate compare, nint nil, nint nil2);
}

delegate bool CustomFilterDelegate(nint data, nint nil);