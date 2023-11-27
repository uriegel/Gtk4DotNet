using System.Runtime.InteropServices;
using Gtk4DotNet.Structs;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public class TextBuffer
{
    public void SetText(string content, int size)
        => SetText(buffer, content, size);

    public string GetText(bool includeHidden = false)
    {
        var s = GetStartIter();
        var e = GetEndIter();
        return GetText(buffer, ref s, ref e, includeHidden).PtrToString(false) ?? "";
    }

    public string GetText(TextIter start, TextIter end, bool includeHidden = false)
        => GetText(buffer, ref start, ref end, includeHidden).PtrToString(false) ?? "";

    public IntPtr CreateTag(string? name = null, string? firstPropertyName = null)
        => CreateTag(buffer, name, firstPropertyName);

    public TextIter GetStartIter()
    {
        GetStartIter(buffer, out var res);
        return res;
    }

    public TextIter GetEndIter()
    {
        GetEndIter(buffer, out var res);
        return res;
    }

    public IntPtr ApplyTag(IntPtr tag, TextIter startIter, TextIter endIter)
        => ApplyTag(buffer, tag, ref startIter, ref endIter);

    public RangeIter SelectRange(RangeIter range)
    {
        var s = range.Start;
        var e = range.End;
        SelectRange(buffer, ref s, ref e);
        return new(s, e);
    }
        
    internal TextBuffer(IntPtr buffer) => this.buffer = buffer; 

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_new", CallingConvention = CallingConvention.Cdecl)]
    extern static TextViewHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetText(IntPtr buffer, string content, int size);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetText(IntPtr buffer, ref TextIter start, ref TextIter end, bool includeHidden);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_create_tag", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr CreateTag(IntPtr buffer, string? name = null, string? firstPropertyName = null);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_get_start_iter", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetStartIter(IntPtr buffer, out TextIter startIter);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_get_end_iter", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetEndIter(IntPtr buffer, out TextIter endIter);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_apply_tag", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr ApplyTag(IntPtr buffer, IntPtr tag, ref TextIter startIter, ref TextIter endIter);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_buffer_select_range", CallingConvention = CallingConvention.Cdecl)]
    extern static void SelectRange(IntPtr buffer, ref TextIter matchStart, ref TextIter matchEnd);

    readonly IntPtr buffer;
}
