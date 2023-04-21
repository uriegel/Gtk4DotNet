using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class CustomSorter
{
    public static IntPtr New(CompareDelegate compare) => New(compare, IntPtr.Zero, IntPtr.Zero);
    
    public delegate int CompareDelegate(IntPtr a, IntPtr b, IntPtr zero);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_custom_sorter_new", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr New(CompareDelegate compare, IntPtr zero, IntPtr zero2);
}

