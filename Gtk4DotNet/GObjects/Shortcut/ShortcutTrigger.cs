using System.Runtime.InteropServices;
using Gtk4DotNet;

public class ShortcutTrigger : GObject
{
    public static ShortcutTrigger ParseString(string str)
    {
        var trigger = _ParseString(str);
        trigger.CheckDiagnostics();
        return trigger;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_trigger_parse_string", CallingConvention = CallingConvention.Cdecl)]
    extern static ShortcutTrigger _ParseString(string str);
}