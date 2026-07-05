using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Paned : Widget
{
    public int Position
    {
        get => GetPosition(this);
        set => SetPosition(this, value);
    }

    public Paned() : base() { }

    public Paned(Builder builder, string? name = null) : base(builder, name) { }

    public Paned(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_get_position", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetPosition(Paned paned);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_paned_set_position", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPosition(Paned paned, int pos);
}
