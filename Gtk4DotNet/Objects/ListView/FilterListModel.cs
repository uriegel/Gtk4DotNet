using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class FilterListModel : ListModel
{
    public static FilterListModel New(ListStore model, Filter? filter)
    {
        var res = New(model, filter?.GetInternalHandle() ?? 0);
        res.CheckDiagnostics();
        filter?.AutoDestroyed = true;
        return res;
    }
        
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    extern static FilterListModel New(ListStore model, nint filter);
}