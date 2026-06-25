using Gtk4DotNet;

class FileView : ScrolledWindow
{
    public FileView() : base() { }

    public FileView(string text, Builder builder, string? name = null) : base(builder, name)
    {
        using var buffer = textview.GetBuffer();
        buffer.SetText(text);
        var tag = buffer.CreateTag(null, null);
        Application.Settings.Bind("font", tag, "font", BindFlags.Default);
        buffer.ApplyTag(tag, buffer.GetStartIter(), buffer.GetEndIter());
    }

    [Widget]
    TextView textview = null!;
}