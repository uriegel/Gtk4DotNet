using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class TextView
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_text_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibGtk, EntryPoint="gtk_text_view_set_editable", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetEditable(IntPtr textView, bool set);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_text_view_set_cursor_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetCursorVisible(IntPtr textView, bool set);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_text_view_get_buffer", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetBuffer(IntPtr textView);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_text_view_scroll_to_iter", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool ScrollToIter(IntPtr textView, ref GtkTextIter iter, double withinMargin, bool useAlign, double xAlign, double yAlign);
}


