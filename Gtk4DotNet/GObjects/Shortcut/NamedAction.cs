using System.Runtime.InteropServices;
using Gtk4DotNet;

public class NamedAction : ShortcutAction
{
    public NamedAction(string name)
    {
        SetInternalHandle(New(name));
        CheckDiagnostics();
    }
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_named_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(string name);
}
