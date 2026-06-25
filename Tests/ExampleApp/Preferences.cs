using Gtk4DotNet;

class Preferences : Dialog
{
    public Preferences(Window parent, Builder builder, string? name = null) : base(builder, name)
    {
        TransientFor(parent);
        Application.Settings.Bind("transition", transition, "active-id", BindFlags.Default);
        Application.Settings.Bind("font", font, "font", BindFlags.Default);
    }

    [Widget]
    ComboBoxText transition = null!;

    [Widget]
    Widget font = null!;
}