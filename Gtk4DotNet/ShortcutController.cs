using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class ShortcutController
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_controller_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ShortcutControllerHandle New();

    public static ShortcutControllerHandle AddShortcut(this ShortcutControllerHandle shortcutController, ShortcutHandle shortcut)
        => shortcutController.SideEffect(c => c._AddShortcut(shortcut));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_controller_add_shortcut", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddShortcut(this ShortcutControllerHandle shortcutController, ShortcutHandle shortcut);
}

