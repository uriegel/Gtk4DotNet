using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Menu
{
    [DllImport(Libs.LibGio, EntryPoint="g_menu_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MenuHandle New();

    public static MenuHandle AppendItem(this MenuHandle menu, MenuItemHandle menuItem)
        => menu.SideEffect(m => m._AppendItem(menuItem));

    [DllImport(Libs.LibGio, EntryPoint="g_menu_append_item", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AppendItem(this MenuHandle menu, MenuItemHandle menuItem);
}

