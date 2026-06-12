using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class MenuButton : Widget
{
    public Arrow Direction
    {
        get => GetDirection(this);
        set => SetDirection(this, value);
    }

    public string? IconName
    {
        get => GetIconName(this).PtrToString(false);
        set => SetIconName(this, value ?? "");
    }

    public static MenuButton New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }

    public void Popup() => Popup(this);
    public void Popdown() => Popdown(this);

    public void SetChild(Widget child) => SetChild(this, child);

    public MenuButton() : base() { }

    public MenuButton(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_new", CallingConvention = CallingConvention.Cdecl)]
    extern static MenuButton _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_popup", CallingConvention = CallingConvention.Cdecl)]
    extern static void Popup(MenuButton button);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_popdown", CallingConvention = CallingConvention.Cdecl)]
    extern static void Popdown(MenuButton button);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_get_direction", CallingConvention = CallingConvention.Cdecl)]
    extern static Arrow GetDirection(MenuButton menuButton);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_set_direction", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDirection(MenuButton menuButton, Arrow arrow);

    // TODO     
    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_set_menu_model", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetModel(MenuButton menuButton, Menu menuModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetIconName(MenuButton menuButton);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(MenuButton menuButton, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_menu_button_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(MenuButton menuButton, Widget child);

    // TODO     
    // [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_popover", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetPopover(MenuButton menuButton, Popover popover);
}

public static class MenuButtonExtensions
{
    public static MenuButton Direction(this MenuButton menuButton, Arrow arrow)
        => menuButton.SideEffect(b => b.Direction = arrow);

    // public static MenuButton Model(MenuButton menuButton, MenuHandle menuModel)
    //     => menuButton.SideEffect(b => b.SetModel(menuModel));

    public static MenuButton IconName(this MenuButton menuButton, string name)
        => menuButton.SideEffect(b => b.IconName = name);

    public static MenuButton Child(this MenuButton menuButton, Widget child)
        => menuButton.SideEffect(b => b.SetChild(child));

    // public static MenuButton Popover(MenuButton menuButton, PopoverHandle popover)
    //     => menuButton.SideEffect(b => b.SetPopover(popover));
}



