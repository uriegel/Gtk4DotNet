using Gtk4DotNet;

class Preferences : Dialog
{
    public Preferences(Window parent, Builder builder, string? name = null) : base(builder, name)
    {
        TransientFor(parent);
        using var settings = GSettings.New(Globals.ApplicationId);
        settings.Bind("transition", transition, "active-id", BindFlags.Default);
        settings.Bind("font", font, "font", BindFlags.Default);
    }

    [Widget]
    ComboBoxText transition = null!;

    [Widget]
    Widget font = null!;
}