using System;
using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class TextBuffer
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static TextViewHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_set_text", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetText(this TextBufferHandle buffer, string content, int size);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_create_tag", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr CreateTag(this TextBufferHandle buffer, string? name = null, string? firstPropertyName = null);
}
