using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class EventControllerKey
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_event_controller_key_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static EventControllerKeyHandle New();

    public static EventControllerKeyHandle OnKeyPressed(this EventControllerKeyHandle eventControllerKey, Func<uint, uint, KeyModifiers, bool> onKeyPressed)
        => eventControllerKey.SideEffect(eck => Gtk.SignalConnect<KeyPressedDelegate>(eck, "key-pressed", 
            (IntPtr _, uint key, uint keyCode, KeyModifiers modifiers, IntPtr __)  => onKeyPressed(key, keyCode, modifiers)));
}

