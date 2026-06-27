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
    /// Install an event that triggers when a key is pressed. Return true, if the key was handled
    /// </summary>
    public event Func<char, KeyModifiers, bool> OnKeyPressed
    {
        add
        {
            KeyPressedDelegate unmanagedDelegate = (_, key, keyCode, modifiers, _) => value(Gtk.KeyValToUnicode(key, keyCode), modifiers);
            var id = SignalConnectForEvent("key-pressed", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_event_controller_key_new", CallingConvention = CallingConvention.Cdecl)]
    extern static KeyEventController _New();
}