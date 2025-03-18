using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class EventControllerKey
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_event_controller_key_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static EventControllerKeyHandle New();

    public static EventControllerKeyHandle OnKeyPressed(this EventControllerKeyHandle eventControllerKey, Func<uint, uint, KeyModifiers, bool> onKeyPressed)
        => eventControllerKey.SideEffect(eck => Gtk.SignalConnect<KeyPressedDelegate>(eck, "key-pressed",
            (nint _, uint key, uint keyCode, KeyModifiers modifiers, nint __) => onKeyPressed(key, keyCode, modifiers)));

    public static EventControllerKeyHandle OnKeyReleased(this EventControllerKeyHandle eventControllerKey, Action<uint, uint, KeyModifiers> onKeyReleased)
        => eventControllerKey.SideEffect(eck => Gtk.SignalConnect<KeyReleasedDelegate>(eck, "key-released",
            (nint _, uint key, uint keyCode, KeyModifiers modifiers, nint __) => onKeyReleased(key, keyCode, modifiers)));

    public static EventControllerKeyHandle OnModifiers(this EventControllerKeyHandle eventControllerKey, Action<KeyModifiers> onModifiers)
        => eventControllerKey.SideEffect(eck => Gtk.SignalConnect<OnModifiersDelegate>(eck, "modifiers", 
            (nint _, KeyModifiers modifiers, nint __)  => onModifiers(modifiers)));
}

