using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Widget
{
    public static THandle Ref<THandle>(this THandle widget, ObjectRef<THandle> ObjectRef)
        where THandle : WidgetHandle, new()
        => widget.SideEffect(w => ObjectRef.Handle = widget);

    // TODO Text cleanup and GC collect
    // public static THandle Show<THandle>(this THandle widget)
    //     where THandle : WidgetHandle
    //     => widget
    //     .SideEffect(w => w._Show())
    //     .SideEffect(w => GC.Collect())
    //     .SideEffect(w => GC.Collect());

    public static THandle Show<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w._Show());

    public static THandle Visible<THandle>(this THandle widget, bool set)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetVisible(set));

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

    public static THandle CssClass<THandle>(this THandle widget, string cssClass)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.AddCssClass(cssClass));

    public static THandle AddController<THandle>(this THandle widget, EventControllerHandle eventController)
        where THandle : WidgetHandle
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

    public static THandle? GetFirstChild<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => _GetFirstChild(widget) as THandle;

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_style_context", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetStyleContext(this WidgetHandle widget);

    public static THandle? GetParent<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => _GetParent(widget) as THandle;

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_sensitive", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetSensitive(this WidgetHandle widget);

    public static THandle Sensitive<THandle>(this THandle widget, bool sensitive)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetSensitive(sensitive));


    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_add_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_remove_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static DisplayHandle GetDisplay(this WidgetHandle widget);

    public static THandle InsertAfter<THandle>(this THandle widget, WidgetHandle child, WidgetHandle? previous = null)
        where THandle : WidgetHandle
        => widget.SideEffect(w => _InsertAfter(child, w, previous ?? new WidgetHandle()));

    /// <summary>
    /// Widgets can be named, which allows you to refer to them from a CSS file. You can apply a style to widgets with a particular name in the CSS file. See the documentation for the CSS syntax (on the same page as the docs for GtkStyleContext).
    /// Note that the CSS syntax has certain special characters to delimit and represent elements in a selector (period, #, >, *…), so using these will make your widget impossible to match by name. Any combination of alphanumeric symbols, dashes and underscores will suffice
    /// </summary>
    /// <typeparam name="THandle"></typeparam>
    /// <param name="widget"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static THandle Name<THandle>(this THandle widget, string name)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetName(name));

    /// <summary>
    /// Widgets can be named, which allows you to refer to them from a CSS file. You can apply a style to widgets with a particular name in the CSS file. See the documentation for the CSS syntax (on the same page as the docs for GtkStyleContext).
    /// Note that the CSS syntax has certain special characters to delimit and represent elements in a selector (period, #, >, *…), so using these will make your widget impossible to match by name. Any combination of alphanumeric symbols, dashes and underscores will suffice
    /// </summary>
    /// <param name="widget"></param>
    /// <returns></returns>
    public static string? GetName(this WidgetHandle widget)
        => widget._GetName().PtrToString(false);

    public static IEnumerable<WidgetHandle> GetChildren(this WidgetHandle widget)
    {
        var first = widget._GetFirstChild();
        if (first?.IsInvalid == true)
            yield break;
        else
            yield return first!;

        if (first?.IsInvalid != true)
        {
            var current = first!;
            while (true)
            {
                var next = current._GetNextSibling();
                if (next.IsInvalid)
                    yield break;
                yield return next;
                current = next;
            }
        }
    }

    public static WidgetHandle? FindWidget(this WidgetHandle widget, Func<WidgetHandle, bool> predicate)
        => widget
            .GetAllChildren()
            .FirstOrDefault(predicate);

    public static IEnumerable<WidgetHandle> GetAllChildren(this WidgetHandle widget)
    {
        var children = widget.GetChildren();
        var childrensChildren = from n in children
                                from m in n.GetAllChildren()
                                select m;
        return children.Concat(childrensChildren);
    }

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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_next_sibling", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle _GetNextSibling(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_get_parent", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle _GetParent(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_add_controller", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddController(this WidgetHandle widget, EventControllerHandle eventController);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_widget_set_sensitive", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSensitive(this WidgetHandle widget, bool sensitive);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_insert_after", CallingConvention = CallingConvention.Cdecl)]
    extern static void _InsertAfter(WidgetHandle widget, WidgetHandle parent, WidgetHandle? previous);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetName(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetName(this WidgetHandle widget, string name);
}

