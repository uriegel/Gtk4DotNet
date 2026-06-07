namespace Gtk4DotNet;


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class WidgetAttribute : Attribute
{
    public WidgetAttribute()
    {
    }

    public string? Name { get; set; }
    public string? Template { get; set; }
}
