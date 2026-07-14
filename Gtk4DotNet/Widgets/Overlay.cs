using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Overlay : Widget
{
    public void SetChild(Widget child) => SetChild(this, child);

    public Overlay() : base() { }

    public Overlay(Builder builder, string? name = null) : base(builder, name) { }

    public Overlay(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_overlay_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(Overlay overlay, Widget child);
}
