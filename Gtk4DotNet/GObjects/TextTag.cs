using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO Release ready

public class TextTag : GObject
{
    public static TextTag New(string? name)
    {
        var res = _New(name);
        res.CheckDiagnostics();
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_text_tag_new", CallingConvention = CallingConvention.Cdecl)]
    extern static TextTag _New(string? name);
}
