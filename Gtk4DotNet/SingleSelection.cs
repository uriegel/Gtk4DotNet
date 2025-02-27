using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SingleSelection
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SingleSelectionHandle New(ListModelHandle model);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_set_selected", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSelected(this SingleSelectionHandle ssh, uint pos);
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_get_selected", CallingConvention = CallingConvention.Cdecl)]
    public extern static uint GetSelected(this SingleSelectionHandle ssh);
}

