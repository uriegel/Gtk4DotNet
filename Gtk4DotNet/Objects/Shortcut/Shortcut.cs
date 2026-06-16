using System.Runtime.InteropServices;
using Gtk4DotNet;

public class Shortcut : GObject
{
    public static Shortcut New(string action, string shortcut)
    {
        var trigger = ShortcutTrigger.ParseString(shortcut);
        var namedAction = NamedAction.New(action);
        return New(trigger, namedAction);
    }

    public static Shortcut New(ShortcutTrigger trigger, ShortcutAction action)
    {
        trigger.IsFloating = true;
        action.IsFloating = true;
        var shortcut = _New(trigger, action);
        shortcut.CheckDiagnostics();
        return shortcut;
    }
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Shortcut _New(ShortcutTrigger trigger, ShortcutAction action);
}