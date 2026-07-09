using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class ClickGesture : SingleGesture
{
    public static ClickGesture New()
    {
        var click = _New();
        click.CheckDiagnostics();
        return click;
    }

    public event Action<int, double, double, KeyModifiers> OnPressed
    {
        add
        {
            PressedGestureDelegate unmanagedDelegate = (_, pressCount, x, y, _) =>
                value(pressCount, x, y, GetEventCurrentState());
            var id = SignalConnectForEvent("pressed", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    public event Action<int, double, double> OnReleased
    {
        add
        {
            PressedGestureDelegate unmanagedDelegate = (_, pressCount, x, y, _) => value(pressCount, x, y);
            var id = SignalConnectForEvent("released", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_gesture_click_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ClickGesture _New();
}