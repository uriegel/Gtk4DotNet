using Gtk4DotNet;

class IconNameItem(Builder builder) : Box(builder, "listitem")
{
    public new string Name
    {
        get => text.Text;
        set => text.Text = value;
    }

    public void SetFromIconName(string name)
        => image.SetFromIconName(name);

    [Widget]
    readonly Image image = null!;

    [Widget]
    readonly Label text = null!;
}

