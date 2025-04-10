using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;
using CsTools.Functional;
using System.ComponentModel;

namespace GtkDotNet;

public static class Widget
{
    public static THandle Ref<THandle>(this THandle widget, ObjectRef<THandle> ObjectRef)
        where THandle : WidgetHandle, new()
        => widget.SideEffect(w => ObjectRef.Handle = widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

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

    public static THandle Margin<THandle>(this THandle widget, int margin)
        where THandle : WidgetHandle
        => widget
            .MarginTop(margin)
            .MarginBottom(margin)
            .MarginStart(margin)
            .MarginEnd(margin);

    public static THandle OnRealize<THandle>(this THandle widget, Action<THandle> onRealize)
        where THandle : WidgetHandle
        => widget.SideEffect(a => Gtk.SignalConnect<TwoPointerDelegate>(a, "realize", (_, ___) => onRealize(widget)));

    public static THandle OnSizeChanged<THandle>(this THandle widget, Action<int, int> onSizeChanged)
        where THandle : WidgetHandle
    {
        var width = -1;
        var height = -1;
        widget.SetTimer(300, TimeSpan.FromMilliseconds(100), () =>
            {
                var w = widget.GetWidth();
                var h = widget.GetHeight();
                if (w != width || h != height)
                {
                    width = w;
                    height = h;
                    onSizeChanged(w, h);
                }
            });
        return widget;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_unparent", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Unparent(this WidgetHandle widget);

    public static void SetTimer(this WidgetHandle widget, int priority, TimeSpan timeout, Action action)
    {
        RefCell<bool> disposed = new(false);
        widget.AddWeakRef(() => disposed.Value = true);
        Gtk.SetTimer(priority, timeout, () =>
            {
                if (!disposed.Value)
                    action();
                return !disposed.Value;
            });
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_hide", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Hide(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetVisible(this WidgetHandle widget, bool visible);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_visible", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetVisible(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_root", CallingConvention = CallingConvention.Cdecl)]
    public extern static WidgetHandle GetRoot(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetWidth(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetHeight(this WidgetHandle widget);

    public static THandle SizeRequest<THandle>(this THandle widget, int width, int height)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetSizeRequest(width, height));

    public static THandle CssClass<THandle>(this THandle widget, string cssClass)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.AddCssClass(cssClass));

    public static void AddCssClass(this WidgetHandle widget, string cssClass, bool add)
    {
        if (add)
            widget.CssClass(cssClass);
        else
            widget.RemoveCssClass(cssClass);
    }

    public static THandle DataContext<THandle>(this THandle widget, INotifyPropertyChanged dataContext)
        where THandle : WidgetHandle, new()
    {
        var gchandle = GCHandle.Alloc(dataContext, GCHandleType.Normal);
        var ptr = GCHandle.ToIntPtr(gchandle);
        widget.SetData(DATA_CONTEXT, ptr);
        widget.AddWeakRef(() =>
        {
            var ptr = widget.GetData(DATA_CONTEXT);
            var gcHandle = GCHandle.FromIntPtr(ptr);
            gcHandle.Free();
        });
        return widget;
    }

    public static INotifyPropertyChanged? GetDataContext(this WidgetHandle widget)
    {
        var w = widget;
        while (true)
        {
            var ptr = w.GetData(DATA_CONTEXT);
            if (ptr != 0)
            {
                var gcHandle = GCHandle.FromIntPtr(ptr);
                return gcHandle.Target as INotifyPropertyChanged;
            }
            w = w.GetParent();
            if (w.IsInvalid)
                return null;
        }
    }

    public static THandle Binding<THandle>(this THandle target, string targetProperty, string property, BindingFlags bindingFlags,
        Func<object?, object?>? converter = null)
            where THandle : WidgetHandle, new()
    {
        Connect();
        return target;

        async void Connect()
        {
            var dataContext = target.GetDataContext();
            if (dataContext == null)
            {
                await Task.Delay(1);
                dataContext = target.GetDataContext();
            }
            if (dataContext != null)
            {
                bool inChange = false;
                target.SetProperty(targetProperty, GetValue());
                dataContext.PropertyChanged += OnChanged;
                target.AddWeakRef(() => dataContext.PropertyChanged -= OnChanged);

                if (bindingFlags.HasFlag(BindingFlags.Bidirectional))
                    target.OnNotify(targetProperty, _ => SetValue());

                void OnChanged(object? sender, PropertyChangedEventArgs e)
                {
                    if (!inChange && e.PropertyName == property)
                        Gtk.BeginInvoke(200, () => target.SetProperty(targetProperty, GetValue()));
                }

                object? GetValue()
                {
                    var type = dataContext.GetType();
                    var propInfo = type?.GetProperty(property);
                    var res = propInfo?.GetValue(dataContext);
                    return converter?.Invoke(res) ?? res;
                }

                void SetValue()
                {
                    inChange = true;
                    var type = dataContext.GetType();
                    var propInfo = type?.GetProperty(property);
                    if (propInfo?.PropertyType != null)
                    {
                        var val = target.GetProperty(targetProperty, propInfo.PropertyType);
                        propInfo?.SetValue(dataContext, val);
                    }
                    inChange = false;
                }
            }
            else
            {
                Console.Error.WriteLine($"Binding not possible: DataContext not set");
            }
        }
    }

    public static THandle BindingToCss<THandle>(this THandle target, string cssClass, string property, Func<object?, bool>? converter = null)
            where THandle : WidgetHandle, new()
    {
        Connect();
        return target;

        async void Connect()
        {
            var dataContext = target.GetDataContext();
            if (dataContext == null)
            {
                await Task.Delay(1);
                dataContext = target.GetDataContext();
            }
            if (dataContext != null)
            {
                target.AddCssClass(cssClass, GetValue());
                dataContext.PropertyChanged += OnChanged;
                target.AddWeakRef(() => dataContext.PropertyChanged -= OnChanged);

                void OnChanged(object? sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == property)
                        target.AddCssClass(cssClass, GetValue());
                }

                bool GetValue()
                {
                    var type = dataContext.GetType();
                    var propInfo = type?.GetProperty(property);
                    var res = propInfo?.GetValue(dataContext);
                    return converter?.Invoke(res) ?? (bool?)res == true;
                }
            }
            else
            {
                Console.Error.WriteLine($"Binding to css not possible: DataContext not set");
            }
        }
    }
    
    public static THandle AddController<THandle>(this THandle widget, EventControllerHandle eventController)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w._AddController(eventController.SideEffect(n => n.IsFloating = true)));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_remove_controller", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveController(this WidgetHandle widget, EventControllerHandle eventController);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_destroy", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Destroy(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_grab_focus", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GrabFocus(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_allocated_width", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedWidth(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_allocated_height", CallingConvention = CallingConvention.Cdecl)]
    public extern static int GetAllocatedHeight(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_queue_draw", CallingConvention = CallingConvention.Cdecl)]
    public extern static void QueueDraw(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_native", CallingConvention = CallingConvention.Cdecl)]
    public extern static NativeHandle GetNative(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_init_template", CallingConvention = CallingConvention.Cdecl)]
    public extern static void InitTemplate(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_class_set_template", CallingConvention = CallingConvention.Cdecl)]
    public extern static void ClassSetTemplate(this nint widgetClass, BytesHandle gbytes);

    public static void ClassSetTemplate(this nint widgetClass, string template)
    {
        using var bytes = GBytes.New(template);
        widgetClass.ClassSetTemplate(bytes);
    }

    public static void ClassSetTemplateFromDotNetResource(this nint widgetClass, string templatePath)
        => widgetClass.ClassSetTemplate(new StreamReader(Resources.Get(templatePath)!).ReadToEnd());

    public static THandle HExpand<THandle>(this THandle widget, bool expand)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetHExpand(expand));

    public static THandle VExpand<THandle>(this THandle widget, bool expand)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetVExpand(expand));

    public static THandle Tooltip<THandle>(this THandle widget, string text)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetTooltipText(text));

    public static THandle GetFirstChild<THandle>(this WidgetHandle widget)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(_GetFirstChild(widget));
        return res;
    }

    public static THandle GetNextSibling<THandle>(this WidgetHandle widget)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(_GetNextSibling(widget));
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_style_context", CallingConvention = CallingConvention.Cdecl)]
    public extern static StyleContextHandle GetStyleContext(this WidgetHandle widget);

    public static WidgetHandle GetParent<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => _GetParent(widget);

    public static TResultHandle GetAncestor<TResultHandle>(this WidgetHandle widget)
        where TResultHandle : WidgetHandle, new()
    {
        string[] ancestorTypeNames =
            typeof(TResultHandle) == typeof(WindowHandle)
            || typeof(TResultHandle) == typeof(ApplicationWindowHandle)
            || typeof(TResultHandle) == typeof(AdwApplicationWindowHandle)
            // TODO add all
            ? ["GtkApplicationWindow", "AdwApplicationWindow", "GtkWindow", Application.MANAGED_ADW_APPLICATION_WINDOW, Application.MANAGED_APPLICATION_WINDOW, "AdwWindow"]
            : typeof(TResultHandle) == typeof(BoxHandle)
            ? ["GtkBox"]
            : typeof(TResultHandle) == typeof(PanedHandle)
            ? ["GtkPaned"]
            : typeof(TResultHandle) == typeof(ScrolledWindowHandle)
            ? ["GtkScrolledWindow"]
            : [];

        return GetAncestor<TResultHandle>(widget, ancestorTypeNames);
    }

    public static WidgetHandle GetAncestor(this WidgetHandle widget, string[] ancestorTypeNames)
            => GetAncestor<WidgetHandle>(widget, ancestorTypeNames);

    public static TResultHandle GetAncestor<TResultHandle>(this WidgetHandle widget, string[] ancesterTypeNames)
        where TResultHandle : WidgetHandle, new()
    {
        while (true)
        {
            var parent = widget.GetParent();
            if (parent.IsInvalid)
                return new TResultHandle();
            if (ancesterTypeNames.Any(n => parent.GetName() == n))
            {
                var res = new TResultHandle();
                res.SetInternalHandle(parent.GetInternalHandle());
                return res;
            }
            widget = parent;
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_sensitive", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetSensitive(this WidgetHandle widget);

    public static THandle Sensitive<THandle>(this THandle widget, bool sensitive)
        where THandle : WidgetHandle
        => widget.SideEffect(w => w.SetSensitive(sensitive));

    public static string? GetBuildableId<THandle>(this THandle widget)
        where THandle : WidgetHandle
        => BuildableGetBuildableId(widget).PtrToString(false);

    public static TResultHandle GetTemplateChild<TResultHandle, THandle>(this THandle widget, string id)
        where THandle : WidgetHandle
        where TResultHandle : WidgetHandle, new()
    {
        var foundWidget = widget.GetAllChildren().FirstOrDefault(n => n.GetBuildableId() == id);
        if (foundWidget != null)
        {
            var res = new TResultHandle();
            res.SetInternalHandle(foundWidget.GetInternalHandle());
            return res;
        }
        else
            return new TResultHandle();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_add_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_remove_css_class", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveCssClass(this WidgetHandle widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_display", CallingConvention = CallingConvention.Cdecl)]
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

    public static THandle OnMap<THandle>(this THandle widget, Action onMap)
        where THandle : WidgetHandle
        => widget.SideEffect(w => Gtk.SignalConnect<TwoPointerDelegate>(w, "map", (_, __) => onMap()));

    // public static WidgetHandle FindChildByName<WidgetHandle>(this WidgetHandle widget, string name)
    // {
    //     if (!parent.IsInvalid)
    //     {
    //     }
    //     else
    //     {
    //         return 0;
    //     }
    // }

    public static IEnumerable<WidgetHandle> GetChildren(this WidgetHandle parent)
    {
        var first = parent.GetFirstWidget();
        if (first?.IsInvalid == true)
            yield break;
        else
            yield return first!;

        if (first?.IsInvalid != true)
        {
            var current = first!;
            while (true)
            {
                var next = current.GetNextWidgetSibling();
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

    internal const string DATA_CONTEXT = "DATA_CONTEXT";

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_show", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Show(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_halign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHAlign(this WidgetHandle widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_valign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetVAlign(this WidgetHandle widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_size_request", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSizeRequest(this WidgetHandle widget, int width, int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_hexpand", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHExpand(this WidgetHandle widget, bool expand);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_vexpand", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetVExpand(this WidgetHandle widget, bool expand);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_start", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginStart(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_end", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginEnd(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_top", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginTop(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_margin_bottom", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetMarginBottom(this WidgetHandle widget, int margin);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_first_child", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetFirstChild(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_first_child", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle GetFirstWidget(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_next_sibling", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetNextSibling(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_next_sibling", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle GetNextWidgetSibling(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_parent", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle _GetParent(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_add_controller", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddController(this WidgetHandle widget, EventControllerHandle eventController);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_sensitive", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSensitive(this WidgetHandle widget, bool sensitive);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_insert_after", CallingConvention = CallingConvention.Cdecl)]
    extern static void _InsertAfter(WidgetHandle widget, WidgetHandle parent, WidgetHandle? previous);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetName(this WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetName(this WidgetHandle widget, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_tooltip_text", CallingConvention = CallingConvention.Cdecl)]
    extern static WidgetHandle SetTooltipText(this WidgetHandle widget, string text);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_buildable_get_buildable_id", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr BuildableGetBuildableId(this WidgetHandle buildable);
}

