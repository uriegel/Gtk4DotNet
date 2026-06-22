using Gtk4DotNet;

class ListItemHeader : Label
{
    public ListItemHeader(Builder builder, string text) 
        : base(builder, "header") 
        => Text = text;
}