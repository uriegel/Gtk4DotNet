using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class CheckButton : Widget
{
    /// <summary>
    /// Is the button checked?
    /// </summary>
    public bool IsActive
    {
        get => GetIsActive(this);
        set => SetIsActive(this, value);
    }

    public static CheckButton NewWithLabel(string label)
    {
        var res = _NewWithLabel(label);
        res.CheckDiagnostics();
        return res;
    }

    public CheckButton() : base() { }

    public CheckButton(Builder builder, string? name = null) : base(builder, name) { }

    public event Action<bool> OnToggled
    {
        add
        {
            TwoPointerDelegate unmanagedDelegate = (_, __) => value(IsActive);
            var id = SignalConnectForEvent("toggled", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    extern static CheckButton _NewWithLabel(string label);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_get_active", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetIsActive(CheckButton button);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_set_active", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SetIsActive(CheckButton button, bool active);
}

