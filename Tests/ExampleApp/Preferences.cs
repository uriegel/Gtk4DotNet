using Gtk4DotNet;

class Preferences : Dialog
{
    public Preferences(Window parent, Builder builder, string? name = null) : base(builder, name)
    {
        TransientFor(parent);
    }
}