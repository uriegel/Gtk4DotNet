using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class SortListModel
{
    public static IntPtr New<T>(ListStore<T> model, IntPtr sorter)
        => New(model.Store, sorter);
    
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_sort_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(IntPtr model, IntPtr sorter);
}
