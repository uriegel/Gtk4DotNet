namespace Gtk4DotNet;


/// <summary>
/// When this Attribute is set to a field in a <see cref="Widget"/> that has a corresponding object in a template.ui then this field will be automatically set to this template object.
/// The object name in the template must correspond to the field name, or can be set in the <see cref="Name"/> property of this attribute
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class WidgetAttribute : Attribute
{
    public WidgetAttribute()
    {
    }

    /// <summary>
    /// You have to set the name of the corresponding template widget if it differs from the fields name.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// When a template.ui contains an object that has its implementation in another template.ui, then you have to specify the .NET resource name of this template.
    /// </summary>
    public string? Template { get; set; }
}
