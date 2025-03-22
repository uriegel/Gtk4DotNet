using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SingleSelection
{
    public static SingleSelectionHandle New(IListModel model) => New(model.GetInternalHandle());
        
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_set_selected", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSelected(this SingleSelectionHandle ssh, int pos);
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_get_selected", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetSelected(this SingleSelectionHandle ssh);
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SingleSelectionHandle New(nint model);
}

