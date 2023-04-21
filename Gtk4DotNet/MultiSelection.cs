using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class MultiSelection
{
    public static IntPtr New<T>(ListStore<T> model) => New(model.Store);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_multi_selection_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(IntPtr listModel);
}

