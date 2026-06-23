using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// Base class for all Gtk4 Widgets
/// </summary>
public class Widget : GObject
{
    #region Properties

    /// <summary>
    /// When built from a template.ui, then this is the name this object is given in the template
    /// </summary>
    public string? Name
    {
        get;
        private set;
    }

    /// <summary>
    /// Sets all 4 Margins at once
    /// </summary>
    public int Margin
    {
        set
        {
            SetMarginStart(this, value);
            SetMarginEnd(this, value);
            SetMarginTop(this, value);
            SetMarginBottom(this, value);
        }
    }

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

    public Align HAlign
    {
        get => GetHAlign(this);
        set => SetHAlign(this, value);
    }

    public Align VAlign
    {
        get => GetVAlign(this);
        set => SetVAlign(this, value);
    }

    public bool Visible
    {
        get => GetVisible(this);
        set => SetVisible(this, value);
    }

    public bool Sensitive
    {
        get => GetSensitive(this);
        set => SetSensitive(this, value);
    }

    public double Opacity
    {
        get => GetOpacity(this);
        set => SetOpacity(this, value);
    }

    public string TooltipText
    {
        get => GetTooltipText(this).PtrToString(true) ?? "";
        set => SetTooltipText(this, value);
    }

    /// <summary>
    /// A DataContext object that can be used for bindings
    /// </summary>
    public INotifyPropertyChanged? DataContext
    {
        get
        {
            var w = this;
            while (true)
            {
                var val = w.GetManagedData<INotifyPropertyChanged>(DATA_CONTEXT);
                if (val != null)
                    return val;
                w = w.GetParent();
                if (w.IsInvalid)
                    return null;
            }
        }
        set => SetManagedData(DATA_CONTEXT, value);
    }

    #endregion

    #region Methods

    public void Show() => Show(this);
    public void Hide() => Hide(this);

    public Widget GetParent() => GetParent(this);

    /// <summary>
    /// Adds (or removes if add = false) a css class to this widget
    /// </summary>
    /// <param name="cssClass"></param>
    /// <param name="add">Adds the css class if true, otherwise removes the css class</param>
    public void AddCssClass(string cssClass, bool add = true)
    {
        if (add)
            AddCssClass(this, cssClass);
        else
            RemoveCssClass(this, cssClass);
    }

    public void GrabFocus() => GrabFocus(this);

    /// <summary>
    /// Sets a binding between this widget and a value in a given and attached DataContext. The DataContext can be set in a parent widget
    /// </summary>
    /// <param name="targetProperty"></param>
    /// <param name="property"></param>
    /// <param name="bindingFlags"></param>
    /// <param name="converter"></param>
    public void SetBinding(string targetProperty, string property,
        BindingFlags bindingFlags = BindingFlags.Default, Func<object?, object?>? converter = null)
    {
        var dataContext = DataContext;
        if (dataContext != null)
        {
            bool inChange = false;
            SetProperty(targetProperty, GetValue());
            dataContext.PropertyChanged += OnChanged;
            AddWeakRef(() => dataContext.PropertyChanged -= OnChanged);

            if (bindingFlags.HasFlag(BindingFlags.Bidirectional))
                OnNotify(targetProperty, SetValue);

            void OnChanged(object? sender, PropertyChangedEventArgs e)
            {
                if (!inChange && e.PropertyName == property)
                    Gtk.BeginInvoke(200, () => SetProperty(targetProperty, GetValue()));
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
                    var val = GetProperty(targetProperty, propInfo.PropertyType);
                    propInfo?.SetValue(dataContext, val);
                }
                inChange = false;
            }
        }
        else
            Console.Error.WriteLine("Binding not possible: DataContext not set");
    }

    /// <summary>
    /// Sets a binding from a value in a given and attached DataContext to a css class of this object. The DataContext can be set in a parent widget
    /// </summary>
    /// <param name="cssClass"></param>
    /// <param name="property"></param>
    /// <param name="converter"></param>
    public void SetBindingToCss(string cssClass, string property, Func<object?, bool>? converter = null)
    {
        var dataContext = DataContext;
        if (dataContext != null)
        {
            AddCssClass(cssClass, GetValue());
            dataContext.PropertyChanged += OnChanged;
            AddWeakRef(() => dataContext.PropertyChanged -= OnChanged);

            void OnChanged(object? sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == property)
                    AddCssClass(cssClass, GetValue());
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
            Console.Error.WriteLine($"Binding to css not possible: DataContext not set");
    }

    /// <summary>
    /// Gets the style Context of this widget
    /// </summary>
    /// <returns></returns>
    public StyleContext GetStyleContext()
    {
        var res = GetStyleContext(this);
        res.AutoDestroyed = true;
        return res;
    }

    public void SetSizeRequest(int width, int height) => SetSizeRequest(this, width, height);

    public void QueueDraw() => QueueDraw(this);

    public Widget GetRoot() => GetRoot(this);

    /// <summary>
    /// Inserts a <see cref="SimpleActionGroup"/> to this Widgets to attach Actions to it.
    /// </summary>
    /// <param name="name">The name of the group</param>
    /// <param name="group">The <see cref="SimpleActionGroup"/> to be included</param>
    public void InsertActionGroup(string name, SimpleActionGroup group) => InsertActionGroup(this, name, group);

    /// <summary>
    /// Adds an event controller to this widget
    /// </summary>
    /// <param name="controller"></param>
    public void AddController(EventController controller)
    {
        controller.AutoDestroyed = true;
        AddController(this, controller);
    }

    /// <summary>
    /// Adds shotcuts to this widget. It attaches a ShortcutController to achieve this
    /// </summary>
    /// <param name="shortcuts"></param>
    public void AddShortcuts(params Shortcut[] shortcuts)
    {
        var shortcutController = ShortcutController.New();
        foreach (var shortcut in shortcuts)
            shortcutController.AddShortcut(shortcut);
        AddController(shortcutController);
    }

    public DelegateId OnRealize(Action action)
        => SignalConnect<TwoPointerDelegate>("realize", (_, __) => action());

    public DelegateId OnUnrealize(Action action)
        => SignalConnect<TwoPointerDelegate>("unrealize", (_, __) => action());

    /// <summary>
    /// Used to register a widget so it can be found by its Gtk handle value. Used for example in a ListBox, when callbacks delivering handles
    /// </summary>
    public void Register()
    {
        widgets.TryAdd(handle, this);
        AddWeakRef(() => widgets.Remove(handle));
    }

    /// <summary>
    /// Gets a registered widgets by its Gtk handle
    /// </summary>
    /// <typeparam name="TWidget"></typeparam>
    /// <param name="widgetKey"></param>
    /// <returns></returns>
    public static TWidget? GetRegistered<TWidget>(nint widgetKey) where TWidget : Widget
        => widgets.TryGetValue(widgetKey, out var val) ? val as TWidget : null;

    #endregion

    #region Constructor

    public Widget() : base()
        => AutoDestroyed = true;

    public Widget(Builder builder, string? name = null) : this()
    {
        if (name != null)
        {
            SetInternalHandle(builder.GetWidgetPtr(name));
            Name = name;
            CheckDiagnostics();
        }

        var widgetFields = GetType()
            .GetFields(System.Reflection.BindingFlags.Instance |
               System.Reflection.BindingFlags.NonPublic |
               System.Reflection.BindingFlags.Public)
            .Select(f => new
            {
                Field = f,
                Attribute = f.GetCustomAttribute<WidgetAttribute>()
            })
            .Where(x => x.Attribute != null);
        foreach (var field in widgetFields)
        {
            var templateElementName = field.Attribute!.Name ?? field.Field.Name;
            var p = builder.GetWidgetPtr(templateElementName);
            if (p != 0)
            {
                var widgetType = field.Field.FieldType;
                var ctor = widgetType.GetConstructor([typeof(Builder), typeof(string)]);
                if (ctor == null)
                {
                    Console.Error.WriteLine(
@$"===================================================
W A R N I N G
{templateElementName} could not be built from template, ctor(Builder, string) is missing
                    
===================================================");
                    continue;
                }
                var instance = (ctor != null
                    ? field.Attribute.Template != null
                    ? CreateInnerWidget(ctor, builder, field.Attribute.Template, templateElementName)
                    : ctor.Invoke([builder, templateElementName])
                    : Activator.CreateInstance(widgetType)) as Widget;
                if (field.Attribute.Template == null)
                    instance?.SetInternalHandle(p);
                field.Field.SetValue(this, instance);
            }
        }

        object CreateInnerWidget(ConstructorInfo ctor, Builder builder, string innerTemplate, string name)
        {
            using var innerBuilder = Builder.FromDotNetResource(innerTemplate);
            var obj = ctor.Invoke([innerBuilder, name]);
            var container = builder.GetWidget<Box>(name);
            if (obj is Widget w)
                container.Append(w);
            return obj;
        }
    }

    #endregion

    #region Internals

    protected override void OnDiagnostics()
        => Console.WriteLine(Name != null ? $"{GetType().Name} {Name} finalized" : $"{GetType().Name} finalized");

    internal const string DATA_CONTEXT = "DATA_CONTEXT";

    internal static int GetRegisteredWidgetCount() => widgets.Count;

    static readonly Dictionary<nint, Widget> widgets = [];

    #endregion

    #region P/Invoke

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_insert_after", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void InsertAfter(Widget widget, Widget parent, Widget? previous);

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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_parent", CallingConvention = CallingConvention.Cdecl)]
    extern static Widget GetParent(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_add_css_class", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddCssClass(Widget widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_remove_css_class", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveCssClass(Widget widget, string cssClass);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_style_context", CallingConvention = CallingConvention.Cdecl)]
    extern static StyleContext GetStyleContext(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_queue_draw", CallingConvention = CallingConvention.Cdecl)]
    extern static void QueueDraw(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_root", CallingConvention = CallingConvention.Cdecl)]
    extern static Widget GetRoot(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_grab_focus", CallingConvention = CallingConvention.Cdecl)]
    extern static void GrabFocus(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_visible", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetVisible(Widget widget, bool visible);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_visible", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetVisible(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_insert_action_group", CallingConvention = CallingConvention.Cdecl)]
    extern static void InsertActionGroup(Widget widget, string name, SimpleActionGroup group);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_add_controller", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddController(Widget widget, EventController controller);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_halign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHAlign(Widget widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_valign", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetVAlign(Widget widget, Align align);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_halign", CallingConvention = CallingConvention.Cdecl)]
    extern static Align GetHAlign(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_valign", CallingConvention = CallingConvention.Cdecl)]
    extern static Align GetVAlign(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_opacity", CallingConvention = CallingConvention.Cdecl)]
    extern static double GetOpacity(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_opacity", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetOpacity(Widget widget, double opacity);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_hide", CallingConvention = CallingConvention.Cdecl)]
    extern static void Hide(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_size_request", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSizeRequest(Widget widget, int width, int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_get_sensitive", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetSensitive(Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_widget_set_sensitive", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSensitive(Widget widget, bool value);

    #endregion
}

public static class WidgetExtensions
{
    /// <summary>
    /// Sets all 4 Margins at once
    /// </summary>
    public static THandle Margin<THandle>(this THandle widget, int margin)
        where THandle : Widget
        => widget.SideEffect(w => w.Margin = margin);

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
    public static THandle HAlign<THandle>(this THandle widget, Align align)
        where THandle : Widget
        => widget.SideEffect(w => w.HAlign = align);

    public static THandle VAlign<THandle>(this THandle widget, Align align)
        where THandle : Widget
        => widget.SideEffect(w => w.VAlign = align);

    public static THandle Tooltip<THandle>(this THandle widget, string text)
        where THandle : Widget
        => widget.SideEffect(w => w.TooltipText = text);

    public static THandle Visible<THandle>(this THandle widget, bool value = true)
        where THandle : Widget
        => widget.SideEffect(w => w.Visible = value);

    /// <summary>
    /// Sets a binding between this widget and a value in a given and attached DataContext. The DataContext can be set in a parent widget
    /// </summary>
    public static THandle Binding<THandle>(this THandle target, string targetProperty, string property, BindingFlags bindingFlags = BindingFlags.Default,
         Func<object?, object?>? converter = null)
             where THandle : Widget
         => target.SideEffect(t => t.SetBinding(targetProperty, property, bindingFlags, converter));

    /// <summary>
    /// Adds a css class to this widget
    /// </summary>
    public static THandle CssClass<THandle>(this THandle widget, string cssClass)
        where THandle : Widget
        => widget.SideEffect(w => w.AddCssClass(cssClass));

    /// <summary>
    /// Used to register a widget so it can be found by its Gtk handle value. Used for example in a ListBox, when callbacks delivering handles
    /// </summary>
    /// <typeparam name="THandle"></typeparam>
    /// <param name="widget"></param>
    /// <returns></returns>
    public static THandle RegisterWidget<THandle>(this THandle widget)
        where THandle : Widget
        => widget.SideEffect(w => w.Register());

    public static THandle Realize<THandle>(this THandle widget, Action action)
        where THandle : Widget
        => widget.SideEffect(w => w.OnRealize(action));

    public static THandle SizeRequest<THandle>(this THandle widget, int width, int height)
        where THandle : Widget
        => widget.SideEffect(w => w.SetSizeRequest(width, height));

    public static THandle InsertAfter<THandle>(this THandle widget, Widget child, Widget? previous = null)
        where THandle : Widget
        => widget.SideEffect(w => Widget.InsertAfter(child, w, previous ?? new Widget()));
}


