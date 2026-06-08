namespace Gtk4DotNet;

public class GType : FloatingObject
{
    public static GType Get(GTypeEnum type)
    {
        return type switch
        {
            GTypeEnum.GObject => Type(),
            GTypeEnum.WebKitWebView => WebView.Type(),
            _ => Type(),
        };
    }
}
