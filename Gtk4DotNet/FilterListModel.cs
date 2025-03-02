using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class FilterListModel
{
    public static ListModelHandle New(IListModel model, CustomFilterHandle? filter)
        => New(model.GetInternalHandle(), filter?.GetInternalHandle() ?? 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_filter_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListModelHandle New(nint model, nint filter);
}