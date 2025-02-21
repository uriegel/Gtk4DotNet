using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class MenuItem
{
    [DllImport(Libs.LibGio, EntryPoint = "g_menu_item_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MenuItemHandle New(string? label, string action);

    [DllImport(Libs.LibGio, EntryPoint = "g_menu_item_new_section", CallingConvention = CallingConvention.Cdecl)]
    public extern static MenuItemHandle NewSection(string? label, MenuHandle menuModel);
}

