using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO Release ready

public class TextBuffer : GObject
{
    public void SetText(string text) => SetText(this, text, text.Length);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_buffer_set_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetText(TextBuffer buffer, string text, int length);
}
