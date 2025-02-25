using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Button
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static ButtonHandle NewWithLabel(string label);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();        

    public static THandle OnClicked<THandle>(this THandle button, Action click)
        where THandle : ButtonHandle
        => button.SideEffect(a => Gtk.SignalConnect<TwoPointerDelegate>(a, "clicked", (_, __) => click()));

    public static THandle Label<THandle>(this THandle button, string label)
        where THandle : ButtonHandle
        => button.SideEffect(b => b.SetLabel(label));

    public static string GetLabel(this ButtonHandle button)
        => _GetLabel(button).PtrToString(false) ?? "";

    public static THandle IconName<THandle>(this THandle button, string iconName)
        where THandle : ButtonHandle    
        => button.SideEffect(b => b.SetIconName(iconName));

    public static string? GetIconName(this ButtonHandle button)
        => _GetIconName(button).PtrToString(false);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_label", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetLabel(ButtonHandle button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLabel(this ButtonHandle button, string label);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetIconName(ButtonHandle button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(this ButtonHandle button, string iconName);
}

