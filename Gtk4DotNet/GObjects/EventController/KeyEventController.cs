using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;
public class KeyEventController : EventController
{
    public static KeyEventController New()
    {
        var controller = _New();
        controller.CheckDiagnostics();
        return controller;
    }

    /// <summary>
    /// Installing a callback that triggers when a key is pressed. Return true, if the key was handled
    /// </summary>
    /// <param name="onKeyPressed">callback that triggers when a key is pressed. Return true, if the key was handled</param>
    public KeyEventController OnKeyPressed(Func<char, KeyModifiers, bool> onKeyPressed)
    {
        SignalConnect<KeyPressedDelegate>("key-pressed",
            (nint _, int key, int keyCode, KeyModifiers modifiers, nint __) => onKeyPressed(Gtk.KeyValToUnicode(key, keyCode), modifiers));
        return this;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_event_controller_key_new", CallingConvention = CallingConvention.Cdecl)]
    extern static KeyEventController _New();
}