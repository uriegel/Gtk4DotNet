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
    public TextView() : base() { }

    public TextView(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_view_get_buffer", CallingConvention = CallingConvention.Cdecl)]
    extern static TextBuffer GetBuffer(TextView textView);
}