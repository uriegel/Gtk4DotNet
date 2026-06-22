using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO Release ready

public class TextView : Widget
{
    public TextBuffer GetBuffer()
    {
        var buffer = GetBuffer(this);
        buffer.AutoDestroyed = true;
        buffer.CheckDiagnostics();
        return buffer;
    }

    public bool ScrollToIter(TextIter iter, double withinMargin = 0, bool useAlign = false, double xAlign = 0, double yAlign = 0)
        => ScrollToIter(this, ref iter, withinMargin, useAlign, xAlign, yAlign);

    public TextView() : base() { }

    public TextView(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_get_buffer", CallingConvention = CallingConvention.Cdecl)]
    extern static TextBuffer GetBuffer(TextView textView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_scroll_to_iter", CallingConvention = CallingConvention.Cdecl)]
    extern static bool ScrollToIter(TextView textView, ref TextIter iter, double withinMargin, bool useAlign, double xAlign, double yAlign);
}