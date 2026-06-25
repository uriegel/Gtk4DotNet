using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class SortListModel : ListModel
{
    public static SortListModel New(ListModel model, CustomSorter? sorter)
    {
        var res = _New(model, sorter?.GetInternalHandle() ?? 0);
        res.CheckDiagnostics();
        model.AutoDestroyed = true;
        sorter?.AutoDestroyed = true;
        return res;
    }
        
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_sort_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SortListModel _New(ListModel model, nint sorter);
}