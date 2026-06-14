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

    public void OnKeyPressed(Func<char, KeyModifiers, bool> onKeyPressed)
        => SignalConnect<KeyPressedDelegate>("key-pressed",
            (nint _, int key, int keyCode, KeyModifiers modifiers, nint __) => onKeyPressed(Gtk.KeyValToUnicode(key, keyCode), modifiers));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_event_controller_key_new", CallingConvention = CallingConvention.Cdecl)]
    extern static KeyEventController _New();
}