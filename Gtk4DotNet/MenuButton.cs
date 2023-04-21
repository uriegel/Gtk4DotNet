using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class MenuButton
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_menu_button_set_menu_model", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr SetModel(IntPtr menuButton, IntPtr menuModel);
}
