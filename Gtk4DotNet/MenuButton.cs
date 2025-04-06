using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class MenuButton
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MenuButtonHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();        

    public static MenuButtonHandle Direction(this MenuButtonHandle menuButton, Arrow arrow)
        => menuButton.SideEffect(b => b.SetDirection(arrow));

    public static MenuButtonHandle Model(this MenuButtonHandle menuButton, MenuHandle menuModel)
        => menuButton.SideEffect(b => b.SetModel(menuModel));

    public static MenuButtonHandle IconName(this MenuButtonHandle menuButton, string name)
        => menuButton.SideEffect(b => b.SetIconName(name));

    public static MenuButtonHandle Child(this MenuButtonHandle menuButton, WidgetHandle child)
        => menuButton.SideEffect(b => b.SetChild(child));

    public static MenuButtonHandle Popover(this MenuButtonHandle menuButton, PopoverHandle popover)
        => menuButton.SideEffect(b => b.SetPopover(popover));
        
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_popup", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Popup(this MenuButtonHandle button);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_popdown", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Popdown(this MenuButtonHandle button);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_set_direction", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDirection(this MenuButtonHandle menuButton, Arrow arrow);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_menu_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModel(this MenuButtonHandle menuButton, MenuHandle menuModel);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(this MenuButtonHandle menuButton, string name);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this MenuButtonHandle menuButton, WidgetHandle child);
     
    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_popover", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetPopover(this MenuButtonHandle menuButton, PopoverHandle popover);
}

