using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Button
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static ButtonHandle NewWithLabel(string label);

    public static ButtonHandle OnClicked(this ButtonHandle button, Action click)
        => button.SideEffect(a => Gtk.SignalConnect<TwoPointerDelegate>(a, "clicked", (IntPtr _, IntPtr __)  => click()));

    public static ButtonHandle Label(this ButtonHandle button, string label)
        => button.SideEffect(b => b.SetLabel(label));

    public static string GetLabel(this ButtonHandle button)
        => _GetLabel(button).PtrToString() ?? "";

    public static ButtonHandle IconName(this ButtonHandle button, string iconName)
        => button.SideEffect(b => b.SetIconName(iconName));

    public static string? GetIconName(this ButtonHandle button)
        => _GetIconName(button).PtrToString();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_label", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetLabel(ButtonHandle button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLabel(this ButtonHandle button, string label);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetIconName(ButtonHandle button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(this ButtonHandle button, string iconName);
}

