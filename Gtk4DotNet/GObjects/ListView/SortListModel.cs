using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class SortListModel : ListModel
{
    public SortListModel(ListModel model, CustomSorter? sorter)
    {
        var handle = New(model, sorter?.GetInternalHandle() ?? 0);
        SetInternalHandle(handle);
        CheckDiagnostics();
        model.AutoDestroyed = true;
        sorter?.AutoDestroyed = true;
    }

    public void SetSorter(Sorter? sorter)
    {
        SetSorter(this, sorter?.GetInternalHandle() ?? 0);
        sorter?.AutoDestroyed = true;
    }

    public Sorter? GetSorter()
    {
        var sh = GetSorter(this);
        if (sh == 0)
            return null;
        return new Sorter(sh);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_sort_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(ListModel model, nint sorter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_sort_list_model_set_sorter", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSorter(ListModel model, nint sorter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_sort_list_model_get_sorter", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetSorter(ListModel model);
}