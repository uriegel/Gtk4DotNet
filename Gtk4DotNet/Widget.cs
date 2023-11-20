using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Widget
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_show", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Show(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_hide", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Hide(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVisible(this WidgetHandle widget, bool visible);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetVisible(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_size_request", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSizeRequest(this WidgetHandle widget, int width, int height);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetWidth(this WidgetHandle widget);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetHeight(this WidgetHandle widget);

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

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_halign", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetHAlign(this WidgetHandle widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_valign", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVAlign(this WidgetHandle widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_add_controller", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddController(this WidgetHandle widget, IntPtr eventController);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_native", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetNative(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_hexpand", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetHExpand(this WidgetHandle widget, bool expand);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_vexpand", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVExpand(this WidgetHandle widget, bool expand);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_sensitive", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSensitive(this WidgetHandle widget, bool sensitive);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_first_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetFirstChild(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_style_context", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetStyleContext(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_parent", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetParent(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_add_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_remove_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetDisplay(this WidgetHandle widget);
}

