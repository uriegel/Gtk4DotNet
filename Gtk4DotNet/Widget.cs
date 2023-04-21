using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Widget
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_show", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Show(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_hide", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Hide(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVisible(IntPtr widget, bool visible);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetVisible(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_size_request", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSizeRequest(IntPtr widget, int width, int height);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetWidth(IntPtr widget);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetHeight(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_destroy", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Destroy(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_grab_focus", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GrabFocus(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_allocated_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedWidth(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_allocated_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedHeight(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_queue_draw", CallingConvention = CallingConvention.Cdecl)]
    public extern static void QueueDraw(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_halign", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetHAlign(IntPtr widget, Align align);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_valign", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVAlign(IntPtr widget, Align align);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_add_controller", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddController(IntPtr widget, IntPtr eventController);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_native", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetNative(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_hexpand", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetHExpand(IntPtr widget, bool expand);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_vexpand", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVExpand(IntPtr widget, bool expand);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_set_sensitive", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSensitive(IntPtr widget, bool sensitive);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_first_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetFirstChild(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_style_context", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetStyleContext(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_parent", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetParent(IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_add_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddCssClass(IntPtr widget, string cssClass);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_remove_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveCssClass(IntPtr widget, string cssClass);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_widget_get_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetDisplay(IntPtr widget);
}

