using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// A button with a hyperlink. It is useful to show quick links to resources.
/// </summary>
public class LinkButton : Button
{
    public LinkButton(string uri, string label) : base(NewWithLabel(uri, label)) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_link_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewWithLabel(string uri, string label);
}
