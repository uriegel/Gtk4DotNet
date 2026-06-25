using System.Runtime.InteropServices;
using Gtk4DotNet;

public class NamedAction : ShortcutAction
{
    public static NamedAction New(string name)
    {
        var action = _New(name);
        action.CheckDiagnostics();
        return action;
    }
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_named_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static NamedAction _New(string name);
}
