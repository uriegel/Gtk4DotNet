namespace GtkDotNet;

using System.Runtime.InteropServices;
using Gtk4DotNet.Structs;

[StructLayout(LayoutKind.Sequential)]
public struct TextIter 
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst=80)] 
    byte[] phantom;

    public RangeIter? ForwardSearch(string text, SearchFlags searchFlags)
        => ForwardSearch(ref this, text, searchFlags, out var matchStart, out var matchEnd, IntPtr.Zero)
            ? new(matchStart, matchEnd)
            : null;

    [DllImport(Libs.LibGtk, EntryPoint="gtk_text_iter_forward_search", CallingConvention = CallingConvention.Cdecl)]
    extern static bool ForwardSearch(ref TextIter start, string text, SearchFlags searchFlags, 
        out TextIter matchStart, out TextIter matchEnd, IntPtr limit);
}


