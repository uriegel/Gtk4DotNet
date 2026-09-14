using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class FilterListModel<T> : ListModel
{
    public FilterListModel(ListStore<T> model, Filter? filter)
    {
        var handle = FilterListModelPinvoke.New(model.GetInternalHandle(), filter?.GetInternalHandle() ?? 0);
        SetInternalHandle(handle);
        CheckDiagnostics();
        filter?.AutoDestroyed = true;
    }

    public void SetFilter(Filter? filter)
    {
        var ptr = filter?.GetInternalHandle() ?? 0;
        FilterListModelPinvoke.SetFilter(GetInternalHandle(), ptr);
        if (ptr != 0)
            Unref(ptr);
        filter?.CheckDiagnostics();
        filter?.AutoDestroyed = true;
    }
}

static class FilterListModelPinvoke
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint New(nint model, nint filter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_list_model_set_filter", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SetFilter(nint model, nint filter);
}