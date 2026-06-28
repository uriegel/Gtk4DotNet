using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class FilterListModel : ListModel
{
    public static FilterListModel New(ListStore model, Filter? filter)
    {
        var res = New(model, filter?.GetInternalHandle() ?? 0);
        res.CheckDiagnostics();
        filter?.WeakCopy = true;
        return res;
    }

    public void SetFilter(Filter? filter)
    {
        var ptr = filter?.GetInternalHandle() ?? 0;
        SetFilter(this, ptr);
        if (ptr != 0)
            Unref(ptr);
        filter?.CheckDiagnostics();
        filter?.WeakCopy = true;
    }
    

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    extern static FilterListModel New(ListStore model, nint filter);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_list_model_set_filter", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFilter(FilterListModel model, nint filter);
}