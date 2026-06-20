namespace Gtk4DotNet;

public class GType : GObject
{
    public static GType Get(GTypeEnum type)
    {
        var res = type switch
        {
            GTypeEnum.GObject => Type(),
            GTypeEnum.WebKitWebView => WebView.Type(),
            _ => Type(),
        };
        res.AutoDestroyed = true;
        return res;
    }
}
