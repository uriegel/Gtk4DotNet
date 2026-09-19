using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// Allows to enter or change numeric values.
/// </summary>
public class SpinButton : Widget
{
    /// <summary>
    /// Creates a new instance of SpinButton
    /// </summary>
    /// <param name="min">Minimum allowable value</param>
    /// <param name="max">Maximum allowable value</param>
    /// <param name="step">Increment added or subtracted by spinning the widget</param>
    /// <returns>The newly created SpinButton</returns>
    public static SpinButton New(double min, double max, double step)
    {
        var res = _New(min, max, step);
        res.CheckDiagnostics();
        res.AutoDestroyed = true;
        return res;
    }

    public event Action OnActivate
    {
        add
        {
            TwoPointerDelegate unmanagedDelegate = (_, __) => value();
            var id = SignalConnectForEvent("activate", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    public SpinButton() : base() { }

    public SpinButton(Builder builder, string? name = null) : base(builder, name) { }

    public SpinButton(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }


    [DllImport(Libs.LibGtk, EntryPoint = "gtk_spin_button_new_with_range", CallingConvention = CallingConvention.Cdecl)]
    extern static SpinButton _New(double min, double max, double step);
}
