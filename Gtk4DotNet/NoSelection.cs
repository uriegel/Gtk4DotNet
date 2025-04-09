using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class NoSelection
{
    public static NoSelectionHandle New(IListModel model) => New(model.GetInternalHandle());
        
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_set_selected", CallingConvention = CallingConvention.Cdecl)]
    extern static NoSelectionHandle New(nint model);
}

