using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// A button with a hyperlink. It is useful to show quick links to resources.
/// </summary>
public class LinkButton : Button
{
    public static LinkButton New(string uri, string label)
    {
        var btn = _NewWithLabel(uri, label);
        btn.CheckDiagnostics();
        return btn;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_link_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    extern static LinkButton _NewWithLabel(string uri, string label);
}
