using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class MenuButton
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MenuButtonHandle New();

    public static MenuButtonHandle Direction(this MenuButtonHandle menuButton, Arrow arrow)
        => menuButton.SideEffect(b => b.SetDirection(arrow));

    public static MenuButtonHandle Model(this MenuButtonHandle menuButton, MenuHandle menuModel)
        => menuButton.SideEffect(b => b.SetModel(menuModel));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_direction", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDirection(this MenuButtonHandle menuButton, Arrow arrow);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_menu_model", CallingConvention = CallingConvention.Cdecl)]
     extern static void SetModel(this MenuButtonHandle menuButton, MenuHandle menuModel);
}

