using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class Widget : FloatingObject
{
    public int MarginStart
    {
        get => GetMarginStart(this);
        set => SetMarginStart(this, value);
    }

    public int MarginEnd
    {
        get => GetMarginEnd(this);
        set => SetMarginEnd(this, value);
    }

    public int MarginTop
    {
        get => GetMarginTop(this);
        set => SetMarginTop(this, value);
    }
    
    public int MarginBottom
    {
        get => GetMarginBottom(this);
        set => SetMarginBottom(this, value);
    }

    public string TooltipText
    {
        get => GetTooltipText(this).PtrToString(true) ?? "";
        set => SetTooltipText(this, value);
    }

    public Widget() : base() { }
    public Widget(nint obj) : base() => SetInternalHandle(obj);

    public void Show() => Show(this);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_show", CallingConvention = CallingConvention.Cdecl)]
    extern static void Show(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_start", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginStart(Widget widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_end", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginEnd(Widget widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_top", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginTop(Widget widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_bottom", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginBottom(Widget widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_margin_start", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetMarginStart(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_margin_end", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetMarginEnd(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_margin_top", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetMarginTop(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_margin_bottom", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetMarginBottom(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_tooltip_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTooltipText(Widget widget, string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_tooltip_text", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetTooltipText(Widget widget);
}

public static class WidgetExtensions
{
    public static THandle MarginStart<THandle>(this THandle widget, int margin)
        where THandle : Widget
        => widget.SideEffect(w => w.MarginStart = margin);

    public static THandle MarginEnd<THandle>(this THandle widget, int margin)
        where THandle : Widget
        => widget.SideEffect(w => w.MarginEnd = margin);

    public static THandle MarginTop<THandle>(this THandle widget, int margin)
        where THandle : Widget
        => widget.SideEffect(w => w.MarginTop = margin);
    public static THandle MarginBottom<THandle>(this THandle widget, int margin)
        where THandle : Widget
        => widget.SideEffect(w => w.MarginBottom = margin);
    public static THandle Tooltip<THandle>(this THandle widget, string text)
        where THandle : Widget
        => widget.SideEffect(w => w.TooltipText = text);
}