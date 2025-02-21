using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Popover
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_popover_new", CallingConvention = CallingConvention.Cdecl)]
    public static extern PopoverHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_popover_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static PopoverHandle Child(this PopoverHandle popover, WidgetHandle child)
            => popover.SideEffect(p => p.SetChild(child));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_popover_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this PopoverHandle popover, WidgetHandle child);
}