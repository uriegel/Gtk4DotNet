using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// TODO Release ready

public class Button : Widget
{
    public string IconName
    {
        get => GetIconName(this).PtrToString(false) ?? "";
        set => SetIconName(this, value);
    }
    
    public static Button NewWithLabel(string label)
    {
        var res = _NewWithLabel(label);
        res.CheckDiagnostics();
        return res;
    }

    public void OnClicked(Action click) => SignalConnect<TwoPointerDelegate>("clicked", (_, __) => click());

    public Button() : base() { }

    public Button(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    extern static Button _NewWithLabel(string label);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_get_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetIconName(Button button);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(Button button, string iconName);
}

public static class ButtonExtensions
{
    public static THandle Clicked<THandle>(this THandle button, Action click)
        where THandle : Button
        => button.SideEffect(a => a.OnClicked(click));

    public static THandle IconName<THandle>(this THandle button, string iconName)
        where THandle : Button    
        => button.SideEffect(b => b.IconName = iconName);

}

