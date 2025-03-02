using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ToggleButton
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ToggleButtonHandle New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static bool Active(this ToggleButtonHandle toggleButton)
        => GetActive(toggleButton);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_set_active", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetActive(this ToggleButtonHandle toggleButton, bool active);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_toggled", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Toggle(this ToggleButtonHandle toggleButton);

    public static THandle OnToggled<THandle>(this THandle button, Action<ToggleButtonHandle> toggled)
        where THandle : ToggleButtonHandle
        => button.SideEffect(a =>
            Gtk.SignalConnect<OnToggledDelegate>(a, "toggled", (t, __) =>
                toggled(new ToggleButtonHandle(t).SideEffect(t => t.IsFloating = true))));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_toggle_button_get_active", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetActive(this ToggleButtonHandle toggleButton);
}

delegate void OnToggledDelegate(nint t, nint _);


 