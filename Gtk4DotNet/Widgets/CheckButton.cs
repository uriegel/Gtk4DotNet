using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// TODO Release ready

public class CheckButton : Widget
{
    public bool IsActive
    {
        get => GetIsActive(this);
        set => SetIsActive(this, value);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static CheckButton NewWithLabel(string label);

    public CheckButton() : base() { }

    public CheckButton(Builder builder, string? name = null) : base(builder, name) { }

    public CheckButton OnToggled(Action<bool> onToggle)
        => this.SideEffect(_ => SignalConnect<TwoPointerDelegate>("toggled", (_, __) => onToggle(IsActive)));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_get_active", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetIsActive(CheckButton button);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_set_active", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SetIsActive(CheckButton button, bool active);
}

