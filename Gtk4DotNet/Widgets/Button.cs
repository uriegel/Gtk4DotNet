using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Button : Widget
{
    public static Button NewWithLabel(string label)
    {
        var res = _NewWithLabel(label);
        res.CheckDiagnostics();
        return res;
    }
    
    public void OnClicked(Action click) => SignalConnect<TwoPointerDelegate>("clicked", (_, __) => click());

    public Button() : base() { }
    public Button(Builder builder, string? name = null) : base(builder, name) { }

    internal Button(Widget widget) : base() => handle = widget.TakeHandle();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    extern static Button _NewWithLabel(string label);
}

public static class ButtonExtensions
{
    public static THandle Clicked<THandle>(this THandle button, Action click)
        where THandle : Button
        => button.SideEffect(a => a.OnClicked(click));
}

