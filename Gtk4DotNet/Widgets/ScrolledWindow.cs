using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ScrolledWindow : Widget
{
    public T? GetChild<T>()
        where T: Widget, new() 
    {
        var t = new T();
        var ptr = GetChild(this);
        if (ptr == 0)
            return null;
        t.SetInternalHandle(ptr);
        t.CheckDiagnostics();
        t.AutoDestroyed = true;
        return t;
    }

    public ScrolledWindow() : base() { }

    public ScrolledWindow(Builder builder, string? name = null) : base(builder, name) { }

    public ScrolledWindow(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_scrolled_window_get_child", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetChild(ScrolledWindow scrolled);
}