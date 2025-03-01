using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Shortcut
{
    public static ShortcutHandle New(string shortcut, Func<bool> action)
    {
        bool RawAction(nint _, nint __, nint ___) => action();
        ThreePointerBoolRetDelegate rawActionDelegate = RawAction;
        GtkDelegates.Add(rawActionDelegate);
        return New(TriggerParse(shortcut), CallbackActionNew(rawActionDelegate, 0, 0));
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ShortcutHandle New(nint shortcut, nint action);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_shortcut_trigger_parse_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint TriggerParse(string shortcut);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_callback_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint CallbackActionNew(ThreePointerBoolRetDelegate callback, nint nil, nint nil2);
}

