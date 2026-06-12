using Gtk4DotNet;

class ListItem : Box
{
    public static ListItem New(GIcon icon, string? text)
    {
        var builder = Builder.FromDotNetResource("listitem");
        return new ListItem(builder, icon, text);
    }
    ListItem(Builder builder, GIcon icon, string? text) : base(builder, "listitem")
    {
        IsFloating = false;
        image.SetIcon(icon);
        this.text.Text = text;
    }

    public ListItem() : base() {}

    public static ListItem New(string? text)
    {
        var builder = Builder.FromDotNetResource("listitem");
        return new ListItem(builder, text);
    }
    ListItem(Builder builder,  string? text) : base(builder, "listitem")
    {
        this.text.Text = text;
    }


    [Widget]
    readonly Image image = null!;

    [Widget]
    readonly Label text = null!;
}

