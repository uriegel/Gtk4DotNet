using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO Release ready

public class TextView : Widget
{
    public static TextView New()
    {
        var res = _New();
        res.CheckDiagnostics();
        res.WeakCopy = true;
        return res;
    }

    public TextBuffer GetBuffer()
    {
        var buffer = GetBuffer(this);
        buffer.WeakCopy = true;
        // Don't call this because TextBuffer can leak when a range is set
        // buffer.CheckDiagnostics();
        return buffer;
    }

    public bool ScrollToIter(TextIter iter, double withinMargin = 0, bool useAlign = false, double xAlign = 0, double yAlign = 0)
        => ScrollToIter(this, ref iter, withinMargin, useAlign, xAlign, yAlign);

    public void ResetBuffer() => SetBuffer(this, 0);

    public TextView() : base() { }

    public TextView(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_get_buffer", CallingConvention = CallingConvention.Cdecl)]
    extern static TextBuffer GetBuffer(TextView textView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_scroll_to_iter", CallingConvention = CallingConvention.Cdecl)]
    extern static bool ScrollToIter(TextView textView, ref TextIter iter, double withinMargin, bool useAlign, double xAlign, double yAlign);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_new", CallingConvention = CallingConvention.Cdecl)]
    extern static TextView _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_set_buffer", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBuffer(TextView textView, nint b);
}
