using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SortListModel
{
    public static ListModelHandle New(IListModel model, CustomSorterHandle? sorter)
        => New(model.GetInternalHandle(), sorter?.GetInternalHandle() ?? 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_sort_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListModelHandle New(nint model, nint sorter);
}