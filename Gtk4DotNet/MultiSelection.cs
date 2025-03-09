using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class MultiSelection
{
    public static MultiSelectionHandle New(IListModel model) => New(model.GetInternalHandle());

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static MultiSelectionHandle New(nint model);
}

