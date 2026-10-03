using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO Release ready

public class TextTag : GObject
{
    public TextTag(string? name)
    {
        SetInternalHandle(New(name));
        CheckDiagnostics();
    }

    internal TextTag() { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_tag_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(string? name);
}
