using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class MultiSelection
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_multi_selection_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MultiSelectionHandle New(ListModelHandle model);
}

