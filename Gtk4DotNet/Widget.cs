using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Widget
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_show", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Show(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_hide", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Hide(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVisible(this IntPtr widget, bool visible);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetVisible(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_size_request", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSizeRequest(this IntPtr widget, int width, int height);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetWidth(this IntPtr widget);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetHeight(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_destroy", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Destroy(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_grab_focus", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GrabFocus(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_allocated_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedWidth(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_allocated_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedHeight(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_queue_draw", CallingConvention = CallingConvention.Cdecl)]
    public extern static void QueueDraw(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_halign", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetHAlign(this IntPtr widget, Align align);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_valign", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVAlign(this IntPtr widget, Align align);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_add_controller", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddController(this IntPtr widget, IntPtr eventController);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_native", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetNative(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_hexpand", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetHExpand(this IntPtr widget, bool expand);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_vexpand", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVExpand(this IntPtr widget, bool expand);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_sensitive", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSensitive(this IntPtr widget, bool sensitive);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_first_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetFirstChild(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_style_context", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetStyleContext(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_parent", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetParent(this IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_add_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddCssClass(this IntPtr widget, string cssClass);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_remove_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveCssClass(this IntPtr widget, string cssClass);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetDisplay(this IntPtr widget);
}

