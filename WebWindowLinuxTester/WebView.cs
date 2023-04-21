using GtkDotNet;
using WebWindowNetCore.Data;

public class WebView : WebWindowNetCore.Base.WebView
{
    public static WebViewBuilder Create()
        => new WebViewBuilder();

    public override int Run(string gtkId = "de.uriegel.WebViewNetCore")
        => Application.Run(gtkId, app =>
        {
            saveBounds = settings?.SaveBounds == true;
            Application.EnableSynchronizationContext();
            GtkDotNet.Timer? timer = null;

            var window = Application.NewWindow(GtkApplication);
            Window.SetTitle(window, settings?.Title);
            Widget.SetSizeRequest(window, 200, 200);
            Window.SetDefaultSize(window, settings!.Width, settings!.Height);
            Window.SetIconFromDotNetResource(window, settings?.ResourceIcon);
            //if (!saveBounds)            
                Widget.Show(window);
            settings = null;
        });

    internal WebView(WebViewBuilder builder)
        => settings = builder.Data;

    WebViewSettings? settings;

    bool saveBounds;
}