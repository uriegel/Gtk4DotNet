namespace GtkDotNet;

[Flags]
public enum ParamFlags : uint
{
    Readable = 1,
    Writable = 2,
    ReadWrite = 3,
    /// <summary>
    /// The parameter will be set upon object construction. See GObject.ObjectClass.constructed for more details.
    /// </summary>
    Construct = 4,
    /// <summary>
    /// The parameter can only be set upon object construction. See GObject.ObjectClass.constructed for more details.
    /// </summary>
    ConstructOnly = 8,
    /// <summary>
    /// Upon parameter conversion (see g_param_value_convert()) strict validation is not required.
    /// </summary>
    LaxValidation = 16,
    /// <summary>
    /// The string used as name when constructing the parameter is guaranteed to remain valid and unmodified for the lifetime of the parameter. Since 2.8.
    /// </summary>
    StaticName = 32,
    /// <summary>
    /// The string used as nick when constructing the parameter is guaranteed to remain valid and unmmodified for the lifetime of the parameter. Since 2.8.
    /// </summary>
    StaticNick = 64,
    /// <summary>
    /// The string used as blurb when constructing the parameter is guaranteed to remain valid and unmodified for the lifetime of the parameter. Since 2.8.
    /// </summary>
    StaticBlurb = 128,
    /// <summary>
    /// Calls to g_object_set_property() for this property will not automatically result in a “notify” signal being emitted: the implementation must call g_object_notify() themselves in case the property actually changes. Since: 2.42.
    /// </summary>
    ExplicitNotify = 1073741824,
    /// <summary>
    /// The parameter is deprecated and will be removed in a future version. A warning will be generated if it is used while running with G_ENABLE_DIAGNOSTIC=1. Since 2.26.
    /// </summary>
    Deprecated = 2147483648
}