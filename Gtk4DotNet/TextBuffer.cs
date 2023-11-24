using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public class TextBuffer
{
    public void SetText(string content, int size)
        => _SetText(buffer, content, size);

    public IntPtr CreateTag(string? name = null, string? firstPropertyName = null)
        => _CreateTag(buffer, name, firstPropertyName);
        
    internal TextBuffer(IntPtr buffer) => this.buffer = buffer; 

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_new", CallingConvention = CallingConvention.Cdecl)]
    extern static TextViewHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetText(IntPtr buffer, string content, int size);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_create_tag", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _CreateTag(IntPtr buffer, string? name = null, string? firstPropertyName = null);

    readonly IntPtr buffer;
}
