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

    [DllImport(Libs.LibGtk, EntryPoint="gtk_menu_button_set_direction", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDirection(this MenuButtonHandle menuButton, Arrow arrow);
}

