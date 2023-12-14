using System;
using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class TextView
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static TextViewHandle New();

    public static TextViewHandle Text(this TextViewHandle textview, string text)
    {
        var buffer = textview.GetBuffer();
        buffer.SetText(text, text.Length);
        return textview;
    }

    public static string GetText(this TextViewHandle textview, bool includeHidden = false)
        => textview.GetBuffer().GetText();

    public static TextBuffer GetBuffer(this TextViewHandle textView)
        => new TextBuffer(textView.GetRawBuffer());

    public static TextViewHandle SetEditable(this TextViewHandle textview, bool set)
        => textview.SideEffect(s => s._SetEditable(set));

    public static TextViewHandle SetCursorVisible(this TextViewHandle textview, bool set)
        => textview.SideEffect(s => s._SetCursorVisible(set));

    public static bool ScrollToIter(this TextViewHandle textView, TextIter iter, double withinMargin = 0, bool useAlign = false, double xAlign = 0, double yAlign = 0)
        => textView._ScrollToIter(ref iter, withinMargin, useAlign, xAlign, yAlign);        

    /// <summary>
    /// Returns the GtkTextBuffer being displayed by this text view.
    /// The reference count on the buffer is not incremented; the caller of this function won’t own a new reference.
    /// </summary>
    /// <param name="textView"></param>
    /// <returns></returns>
    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_get_buffer", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetRawBuffer(this TextViewHandle textView);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_set_editable", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetEditable(this TextViewHandle textView, bool set);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_set_cursor_visible", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetCursorVisible(this TextViewHandle textView, bool set);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_view_scroll_to_iter", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _ScrollToIter(this TextViewHandle textView, ref TextIter iter, double withinMargin, bool useAlign, double xAlign, double yAlign);
}


