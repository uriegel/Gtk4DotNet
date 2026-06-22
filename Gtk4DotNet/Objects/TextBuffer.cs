using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class TextBuffer : GObject
{
    public int LineCount { get => GetLineCount(this); }
    
    public void SetText(string text) => SetText(this, text, text.Length);

    public string GetText(TextIter start, TextIter end, bool includeHiddenChars)
        => GetText(this, ref start, ref end, includeHiddenChars).PtrToString(true) ?? "";

    public TextTag CreateTag(string? name, string? firstProperty)
    {
        var res = CreateTag(this, name, firstProperty);
        res.AutoDestroyed = true;
        res.CheckDiagnostics();
        return res;
    }

    public void ApplyTag(TextTag tag, TextIter startIter, TextIter endIter)
        => ApplyTag(this, tag, ref startIter, ref endIter);

    public TextIter GetStartIter()
    {
        GetStartIter(this, out var res);
        return res;
    }

    public TextIter GetEndIter()
    {
        GetEndIter(this, out var res);
        return res;
    }

    public void SelectRange(int startPos, int endPos)
    {
        var start = new TextIter();
        GetIterAtOffset(this, ref start, startPos);
        var end = new TextIter();
        GetIterAtOffset(this, ref end, endPos);
        SelectRange(this, ref start, ref end);
    }

    public void SelectRange(RangeIter range)
    {
        var s = range.Start;
        var e = range.End;
        SelectRange(this, ref s, ref e);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetText(TextBuffer buffer, string text, int length);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_create_tag", CallingConvention = CallingConvention.Cdecl)]
    extern static TextTag CreateTag(TextBuffer buffer, string? name, string? firstProperty);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_apply_tag", CallingConvention = CallingConvention.Cdecl)]
    extern static void ApplyTag(TextBuffer buffer, TextTag tag, ref TextIter startIter, ref TextIter endIter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_get_start_iter", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetStartIter(TextBuffer buffer, out TextIter startIter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_get_end_iter", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetEndIter(TextBuffer buffer, out TextIter endIter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_select_range", CallingConvention = CallingConvention.Cdecl)]
    extern static void SelectRange(TextBuffer buffer, ref TextIter matchStart, ref TextIter matchEnd);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_get_iter_at_offset", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetIterAtOffset(TextBuffer buffer, ref TextIter iter, int offset);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetText(TextBuffer buffer, ref TextIter start, ref TextIter end, bool includeHiddenChars);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_get_line_count", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetLineCount(TextBuffer buffer);
}
