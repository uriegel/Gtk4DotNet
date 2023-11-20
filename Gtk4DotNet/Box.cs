using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Box
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_box_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static BoxHandle New(Orientation orientation, int spacing = 0);

    public static BoxHandle Append(this BoxHandle box, WidgetHandle widget)
        => box.SideEffect(b => b._Append(widget));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_box_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Append(this BoxHandle box, WidgetHandle widget);
}

