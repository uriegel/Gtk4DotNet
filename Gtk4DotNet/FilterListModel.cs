using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

// TODO GTK4
public static class FilterListModel
{
    public static IntPtr New<T>(ListStore<T> model, IntPtr filter)
        => New(model.Store, filter);
    
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_filter_list_model_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(IntPtr listModel, IntPtr filter);
}