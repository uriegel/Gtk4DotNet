using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ShortcutController : EventController
{
    public static ShortcutController New()
    {
        var controller = _New();
        controller.CheckDiagnostics();
        return controller;
    }

    public void AddShortcut(Shortcut shortcut)
    {
        shortcut.WeakCopy = true;
        AddShortcut(this, shortcut);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_controller_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ShortcutController _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_controller_add_shortcut", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddShortcut(ShortcutController controller, Shortcut shortcut);
}