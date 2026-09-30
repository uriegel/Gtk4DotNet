using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class Box : Widget
{
    public int Spacing
    {
        get => GetSpacing(this);
        set => SetSpacing(this, value);
    }
    public Box(Orientation orientation, int spacing = 0) : base()
    {
        var handle = New(orientation, spacing);
        SetInternalHandle(handle);
        CheckDiagnostics();
    }

    public Box(Builder builder, string? name = null) : base(builder, name) { }

    public Box(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    public Box Append(Widget widget)
    {
        Append(this, widget);
        return this;
    }

    public Box SetSpacing(int spacing)
        => this.SideEffect(_ => SetSpacing(this, spacing));

    internal Box() : base() {}

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(Orientation orientation, int spacing = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(Box box, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_get_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetSpacing(Box box);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_set_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSpacing(Box box, int spacing);
}

