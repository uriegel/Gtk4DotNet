using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class SingleSelection
{
    public static IntPtr New<T>(ListStore<T> model) => New(model.Store);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_single_selection_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(IntPtr listModel);
}

