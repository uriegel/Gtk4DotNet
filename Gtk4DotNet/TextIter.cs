using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;
public class TextIter
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_text_iter_forward_search", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool ForwardSearch(ref GtkTextIter start, string text, SearchFlags searchFlags, 
        out GtkTextIter matchStart, out GtkTextIter matchSEnd, IntPtr limit);
}
