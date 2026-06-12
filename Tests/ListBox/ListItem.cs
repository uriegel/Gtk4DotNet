using Gtk4DotNet;

class ListItem : Box
{
    public static ListItem New(nint icon, string? text)
    {
        var builder = Builder.FromDotNetResource("listitem");
        return new ListItem(builder, icon, text);
    }
    ListItem(Builder builder, nint icon, string? text) : base(builder, "listitem")
    {
        image.SetGIcon(icon);
        this.text.Text = text;
    }
    [Widget]
    readonly Image image = null!;

    [Widget]
    readonly Label text = null!;
}

