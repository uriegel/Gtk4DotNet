using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ShortcutController : EventController
{
    public ShortcutController()
    {
        SetInternalHandle(New());
        CheckDiagnostics();
    }

    public void AddShortcut(Shortcut shortcut)
    {
        shortcut.AutoDestroyed = true;
        AddShortcut(this, shortcut);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_controller_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_controller_add_shortcut", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddShortcut(ShortcutController controller, Shortcut shortcut);
}