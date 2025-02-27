using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SingleSelection
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_single_selection_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SingleSelectionHandle New(ListModelHandle model);
}

