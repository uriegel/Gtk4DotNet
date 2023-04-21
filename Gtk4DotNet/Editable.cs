using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Editable
{
    public static string GetText(IntPtr editable)
        => Marshal.PtrToStringUTF8(_GetText(editable))!;

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetText(IntPtr editable);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr SetText(IntPtr editable, string text);
}

