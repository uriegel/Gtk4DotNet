using System.Reflection;

namespace Gtk4DotNet;

public class ApplicationWindow : Window // , IActionMap
{
    public ApplicationWindow() : base() { }
    
    public ApplicationWindow(WindowBuilder builder) : base()
    {
        builder.Builder.GetWindow(this, builder.Window);
        SetApplication(this, builder.Application);
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
            var p = builder.Builder.GetWidgetPtr(field.Attribute!.Name ?? field.Field.Name);
            if (p != 0)
            {
                var widgetType = field.Field.FieldType;
                var instance = Activator.CreateInstance(widgetType) as Widget;
                instance?.SetInternalHandle(p);
                field.Field.SetValue(this, instance);
            }
        }
    }

    public ApplicationWindow(nint obj) : base() => SetInternalHandle(obj);

    internal ApplicationWindow(Widget widget) : base() => handle = widget.TakeHandle();
}


