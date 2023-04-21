using System.Diagnostics;
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

            var window = Application.NewWindow(app);
            Window.SetTitle(window, settings?.Title);
            Widget.SetSizeRequest(window, 200, 200);
            Window.SetDefaultSize(window, settings!.Width, settings!.Height);
            Window.SetIconFromDotNetResource(window, settings?.ResourceIcon);

            var webView = WebKit.New();
            if (settings?.DevTools == true)
                GObject.SetBool(WebKit.GetSettings(webView), "enable-developer-extras", true);
            var url = Debugger.IsAttached && !string.IsNullOrEmpty(settings?.DebugUrl)
                ? settings?.DebugUrl
                : settings?.Url != null
                ? settings.Url
                : $"http://localhost:{settings?.HttpSettings?.Port ?? 80}{settings?.HttpSettings?.WebrootUrl}/{settings?.HttpSettings?.DefaultHtml}";
            WebKit.LoadUri( webView, url + settings?.Query ?? "");
            Window.SetChild(window, webView);    

            //if (!saveBounds)            
                Widget.Show(window);
            settings = null;
        });

    internal WebView(WebViewBuilder builder)
        => settings = builder.Data;

    WebViewSettings? settings;

    bool saveBounds;
}