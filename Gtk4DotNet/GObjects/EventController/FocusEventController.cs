using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class FocusEventController : EventController
{
    public static FocusEventController New()
    {
        var controller = _New();
        controller.CheckDiagnostics();
        return controller;
    }

    public event Action OnEnter
    {
        add
        {
            TwoPointerDelegate unmanagedDelegate = (_, __) => value();
            var id = SignalConnectForEvent("enter", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    public event Action OnLeave
    {
        add
        {
            TwoPointerDelegate unmanagedDelegate = (_, __) => value();
            var id = SignalConnectForEvent("leave", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    /// <summary>
    /// Installing a callback that triggers when a key is pressed. Return true, if the key was handled
    /// </summary>
    /// <param name="onKeyPressed">callback that triggers when a key is pressed. Return true, if the key was handled</param>
    public FocusEventController OnKeyPressed(Func<char, KeyModifiers, bool> onKeyPressed)
    {
        SignalConnect<KeyPressedDelegate>("key-pressed",
            (nint _, int key, int keyCode, KeyModifiers modifiers, nint __) => onKeyPressed(Gtk.KeyValToUnicode(key, keyCode), modifiers));
        return this;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_event_controller_focus_new", CallingConvention = CallingConvention.Cdecl)]
    extern static FocusEventController _New();
}