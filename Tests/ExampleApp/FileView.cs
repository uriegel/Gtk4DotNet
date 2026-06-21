using Gtk4DotNet;

class FileView : ScrolledWindow
{
    public FileView() : base() { }

    public FileView(string text, Builder builder, string? name = null) : base(builder, name)
    {
        var buffer = textview.GetBuffer();
        buffer.SetText(text);
    }

    [Widget]
    TextView textview = null!;
}