using System.Runtime.InteropServices;
using Gtk4DotNet;

public class Shortcut : GObject
{
    public Shortcut(string action, string shortcut) 
        : this(ShortcutTrigger.ParseString(shortcut), new NamedAction(action)) { }
    
    public Shortcut(ShortcutTrigger trigger, ShortcutAction action)
    {
        trigger.AutoDestroyed = true;
        action.AutoDestroyed = true;
        SetInternalHandle(New(trigger, action));
        CheckDiagnostics();
    }
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(ShortcutTrigger trigger, ShortcutAction action);
}