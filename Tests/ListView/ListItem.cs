using Gtk4DotNet;

class ListItem : Box
{
    public new string? Name { get => text.Text; }

    public bool IsRecommended { get; }

    public ListItem(Builder builder, GIcon icon, string? text, bool isRecommended = false) : base(builder, "listitem")
    {
        IsRecommended = isRecommended;
        image.SetIcon(icon);
        this.text.Text = text;
    }

    [Widget]
    readonly Image image = null!;

    [Widget]
    readonly Label text = null!;
}

