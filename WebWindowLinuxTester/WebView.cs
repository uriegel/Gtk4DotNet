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

            var display = Widget.GetDisplay(window);
            var theme = Display.IconThemeForDisplay(display);
            IconTheme.AddSearchPath(theme, "/home/uwe"); // /home/uwe/hicolor/48x48/apps/kirk.png
            var hasIt = IconTheme.HasIcon(theme, "kirk");
            Window.SetIconName(window, "kirk");

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