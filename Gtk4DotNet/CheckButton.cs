using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class CheckButton
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static CheckButtonHandle NewWithLabel(string label);

    public static CheckButtonHandle OnToggled(this CheckButtonHandle button, Action<CheckButtonHandle> onToggle)
        => button.SideEffect(b => Gtk.SignalConnect<TwoPointerDelegate>(b, "toggled", (_, __) => onToggle(b)));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_check_button_get_active", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsActive(this CheckButtonHandle button);
}
