using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

// TODO Release ready

public class Box : Widget
{
    public int Spacing
    {
        get => GetSpacing(this);
        set => SetSpacing(this, value);
    }
    public static Box New(Orientation orientation, int spacing = 0)
    {
        var res = _New(orientation, spacing);
        res.CheckDiagnostics();
        return res;
    }

    public Box() : base() { }

    public Box(Builder builder, string? name = null) : base(builder, name) { }

    public Box Append(Widget widget)
    {
        Append(this, widget);
        return this;
    }

    public Box SetSpacing(int spacing)
        => this.SideEffect(_ => SetSpacing(this, spacing));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static Box _New(Orientation orientation, int spacing = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(Box box, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_get_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetSpacing(Box box);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_box_set_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSpacing(Box box, int spacing);
}

