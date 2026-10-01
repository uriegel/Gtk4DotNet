using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ScrolledWindow : Widget
{
    public Widget? GetChild()
    {
        var ptr = GetChild(this);
        if (ptr == 0)
            return null;
        var t = new Widget();
        t.SetInternalHandle(ptr);
        t.CheckDiagnostics();
        t.AutoDestroyed = true;
        return t;
    }

    public ScrolledWindow() : base() { }

    public ScrolledWindow(Builder builder, string? name = null) : base(builder, name) { }

    public ScrolledWindow(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    internal ScrolledWindow(nint handle) : base()
    {
        SetInternalHandle(handle);
        CheckDiagnostics();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_scrolled_window_get_child", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetChild(ScrolledWindow scrolled);
}