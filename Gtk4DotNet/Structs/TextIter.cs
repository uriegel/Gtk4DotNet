namespace Gtk4DotNet;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct TextIter
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
    byte[] phantom;

    public RangeIter? ForwardSearch(string text, SearchFlags searchFlags)
        => ForwardSearch(ref this, text, searchFlags, out var matchStart, out var matchEnd, 0)
            ? new(matchStart, matchEnd)
            : null;

    public RangeIter? ForwardSearch(string text, SearchFlags searchFlags, TextIter finish)
        => ForwardSearch(ref this, text, searchFlags, out var matchStart, out var matchEnd, ref finish)
            ? new(matchStart, matchEnd)
            : null;

    public bool IsEnd() => IsEnd(ref this);

    public bool StartsWord() => StartsWord(ref this);

    public bool ForwardChar() => ForwardChar(ref this);

    public bool ForwardWordEnd() => ForwardWordEnd(ref this);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_iter_forward_search", CallingConvention = CallingConvention.Cdecl)]
    extern static bool ForwardSearch(ref TextIter start, string text, SearchFlags searchFlags,
        out TextIter matchStart, out TextIter matchEnd, nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_iter_forward_search", CallingConvention = CallingConvention.Cdecl)]
    extern static bool ForwardSearch(ref TextIter start, string text, SearchFlags searchFlags,
        out TextIter matchStart, out TextIter matchEnd, ref TextIter iter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_iter_is_end", CallingConvention = CallingConvention.Cdecl)]
    extern static bool IsEnd(ref TextIter iter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_iter_starts_word", CallingConvention = CallingConvention.Cdecl)]
    extern static bool StartsWord(ref TextIter iter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_iter_forward_char", CallingConvention = CallingConvention.Cdecl)]
    extern static bool ForwardChar(ref TextIter iter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_iter_forward_word_end", CallingConvention = CallingConvention.Cdecl)]
    extern static bool ForwardWordEnd(ref TextIter iter);
}


