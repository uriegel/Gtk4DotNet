using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Widget
{
    public static THandle Ref<THandle>(this THandle widget, WidgetRef<THandle> widgetRef)
        where THandle : WidgetHandle, new()
        => widget.SideEffect(w => widgetRef.Handle = widget);

    public static THandle Show<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w._Show());

    public static THandle HAlign<THandle>(this THandle widget, Align align)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetHAlign(align));

    public static THandle VAlign<THandle>(this THandle widget, Align align)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetVAlign(align));

    public static THandle MarginStart<THandle>(this THandle widget, int margin)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetMarginStart(margin));

    public static THandle MarginEnd<THandle>(this THandle widget, int margin)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetMarginEnd(margin));
    
    public static THandle MarginTop<THandle>(this THandle widget, int margin)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetMarginTop(margin));
    public static THandle MarginBottom<THandle>(this THandle widget, int margin)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetMarginBottom(margin));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_hide", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Hide(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVisible(this WidgetHandle widget, bool visible);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetVisible(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetWidth(this WidgetHandle widget);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetHeight(this WidgetHandle widget);

    public static THandle SizeRequest<THandle>(this THandle widget, int width, int height)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetSizeRequest(width, height));

    public static WidgetHandle AddController(this WidgetHandle widget, EventControllerHandle eventController)
        => widget.SideEffect(w => w._AddController(eventController));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_destroy", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Destroy(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_grab_focus", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GrabFocus(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_allocated_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedWidth(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_allocated_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedHeight(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_queue_draw", CallingConvention = CallingConvention.Cdecl)]
    public extern static void QueueDraw(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_native", CallingConvention = CallingConvention.Cdecl)]
    public extern static NativeHandle GetNative(this WidgetHandle widget);

    public static THandle HExpand<THandle>(this THandle widget, bool expand)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetHExpand(expand));

    public static THandle VExpand<THandle>(this THandle widget, bool expand)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetVExpand(expand));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_sensitive", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSensitive(this WidgetHandle widget, bool sensitive);

    public static THandle? GetFirstChild<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => _GetFirstChild(widget) as THandle;

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_style_context", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetStyleContext(this WidgetHandle widget);

    public static THandle? GetParent<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => _GetParent(widget) as THandle;

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_add_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_remove_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static DisplayHandle GetDisplay(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_show", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Show(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_halign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHAlign(this WidgetHandle widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_valign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetVAlign(this WidgetHandle widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_size_request", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSizeRequest(this WidgetHandle widget, int width, int height);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_hexpand", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHExpand(this WidgetHandle widget, bool expand);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_vexpand", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetVExpand(this WidgetHandle widget, bool expand);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_margin_start", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginStart(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_margin_end", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginEnd(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_margin_top", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginTop(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_margin_bottom", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginBottom(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_first_child", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle _GetFirstChild(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_parent", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle _GetParent(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_add_controller", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddController(this WidgetHandle widget, EventControllerHandle eventController);
}

