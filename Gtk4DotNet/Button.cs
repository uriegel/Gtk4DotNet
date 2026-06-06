using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Button : Widget
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_button_new_with_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static Button NewWithLabel(string label);

    public void OnClicked(Action click) => SignalConnect<TwoPointerDelegate>("clicked", (_, __) => click());

    public Button() : base() { }
    public Button(nint obj) : base() => SetInternalHandle(obj);

    internal Button(Widget widget) : base() => handle = widget.TakeHandle();
}

public static class ButtonExtensions
{
    public static THandle Clicked<THandle>(this THandle button, Action click)
        where THandle : Button
        => button.SideEffect(a => a.OnClicked(click));
}

