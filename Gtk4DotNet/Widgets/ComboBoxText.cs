namespace Gtk4DotNet;

// TODO Release ready

public class ComboBoxText : Widget
{
    public ComboBoxText() : base() { }

    public ComboBoxText(Builder builder, string? name = null) : base(builder, name) { }

    public ComboBoxText(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }
}
