using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Editable
{
    public static string EditableGetText(this IntPtr editable)
        => Marshal.PtrToStringUTF8(_GetText(editable))!;

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_editable_set_text", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr EditableSetText(this IntPtr editable, string text);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_editable_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetText(IntPtr editable);
}

