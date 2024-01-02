using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Box
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_box_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static BoxHandle New(Orientation orientation, int spacing = 0);

    public static BoxHandle Append(this BoxHandle box, WidgetHandle widget)
        => box.SideEffect(b => b._Append(widget));

    public static BoxHandle Spacing(this BoxHandle box, int spacing)
        => box.SideEffect(b => b.SetSpacing(spacing));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_box_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Append(this BoxHandle box, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_box_set_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSpacing(this BoxHandle box, int spacing);
}

