using GtkDotNet;
using WebWindowNetCore.Data;

public class WebView : WebWindowNetCore.Base.WebView
{
    public static WebViewBuilder Create()
        => new WebViewBuilder();

    public override int Run(string gtkId = "de.uriegel.WebViewNetCore")
    {
        var app = Application.New(gtkId);
        Action onActivate = () =>
        {
            var window = Application.NewWindow(app);
            Window.SetTitle(window, settings?.Title);
            Widget.SetSizeRequest(window, 200, 200);
            Window.SetDefaultSize(window, settings!.Width, settings!.Height);
            Window.SetIconFromDotNetResource(window, settings?.ResourceIcon);
            Widget.Show(window);
        };

        var status = Application.Run(app, onActivate);
        GObject.Unref(app);
        return status;
    }

    internal WebView(WebViewBuilder builder)
        => settings = builder.Data;

    WebViewSettings? settings;

    bool saveBounds;
}