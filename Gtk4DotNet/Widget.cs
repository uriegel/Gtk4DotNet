using System.ComponentModel;
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

    public INotifyPropertyChanged? DataContext
    {
        get
        {
            var w = this;
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
        set
        {
            var gchandle = GCHandle.Alloc(value, GCHandleType.Normal);
            var ptr = GCHandle.ToIntPtr(gchandle);
            SetData(DATA_CONTEXT, ptr);
            AddWeakRef(() =>
            {
                var ptr = GetData(DATA_CONTEXT);
                var gcHandle = GCHandle.FromIntPtr(ptr);
                gcHandle.Free();
            });
        }
    }

    public void Show() => Show(this);

    public Widget GetParent() => GetParent(this);


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
                this.OnNotify(targetProperty, _ => SetValue());

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
        {
            Console.Error.WriteLine("Binding not possible: DataContext not set");
        }
    }

    public Widget() : base() { }

    public Widget(nint obj) : base() => SetInternalHandle(obj);

    internal const string DATA_CONTEXT = "DATA_CONTEXT";

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

    // TODO
    // public static THandle Binding<THandle>(this THandle target, string targetProperty, string property, BindingFlags bindingFlags,
    //     Func<object?, object?>? converter = null)
    //         where THandle : WidgetHandle, new()

}