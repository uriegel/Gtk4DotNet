using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using GtkDotNet;
using LinqTools;
using WebWindowNetCore;
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
            WebKit.LoadUri(webView, url + settings?.Query ?? "");
            Window.SetChild(window, webView);

            if (!saveBounds)
                Widget.Show(window);
            else
                WebKit.RunJavascript(webView,
                    """ 
                        const bounds = JSON.parse(localStorage.getItem('window-bounds') || '{}')
                        const isMaximized = localStorage.getItem('isMaximized')
                        if (bounds.width && bounds.height)
                            alert(JSON.stringify({action: 2, width: bounds.width, height: bounds.height, isMaximized: isMaximized == 'true'}))
                        else
                            alert(JSON.stringify({action: 2}))
                    """);
            

            var showDevTools = settings?.DevTools == true;
            var withFetch = (settings?.HttpSettings?.RequestDelegates?.Length ?? 0) > 0;

            Gtk.SignalConnect<TwoIntPtr>(webView, "load-changed", (_, e) =>
            {
                if ((WebKitLoadEvent)e == WebKitLoadEvent.WEBKIT_LOAD_COMMITTED)
                {
                    if (saveBounds)
                        WebKit.RunJavascript(webView,
                            """ 
                                const bounds = JSON.parse(localStorage.getItem('window-bounds') || '{}')
                                const isMaximized = localStorage.getItem('isMaximized')
                                if (bounds.width && bounds.height)
                                    alert(JSON.stringify({action: 2, width: bounds.width, height: bounds.height, isMaximized: isMaximized == 'true'}))
                                else
                                    alert(JSON.stringify({action: 2}))
                            """);
                    // if (showDevTools == true)
                    //     WebKit.RunJavascript(webView,
                    //         """ 
                    //             function webViewShowDevTools() {
                    //                 alert(JSON.stringify({action: 1}))
                    //             }
                    //         """);
                    if (withFetch)
                        WebKit.RunJavascript(webView,
                            """ 
                                async function webViewRequest(method, input) {
                                    const msg = {
                                        method: 'POST',
                                        headers: { 'Content-Type': 'application/json' },
                                        body: JSON.stringify(input)
                                    }

                                    const response = await fetch(`/request/${method}`, msg) 
                                    return await response.json() 
                                }
                            """);
                }
            });

            Gtk.SignalConnect<TwoIntPtr>(webView, "script-dialog", (_, d) =>
            {
                var msg = WebKit.ScriptDialogGetMessage(d);
                var text = Marshal.PtrToStringUTF8(msg);

                Console.WriteLine(text);

                var action = JsonSerializer.Deserialize<ScriptAction>(text ?? "", JsonDefault.Value);
                switch (action?.Action)
                {
                    case Action.DevTools:
                        WebKit.InspectorShow(webView);
                        break;
                    case Action.Show:
                        if (action.Width.HasValue && action.Height.HasValue)
                            Window.SetDefaultSize(window, action.Width.Value, action.Height.Value);
                        if (action?.IsMaximized == true)
                            Window.Maximize(window);
                        Widget.Show(window);   
                        break;
                }
            });

            settings = null;
        });

    internal WebView(WebViewBuilder builder)
        => settings = builder.Data;

    WebViewSettings? settings;

    bool saveBounds;
}

enum Action
{
    DevTools = 1,
    Show,
}

record ScriptAction(Action Action, int? Width, int? Height, bool? IsMaximized);
