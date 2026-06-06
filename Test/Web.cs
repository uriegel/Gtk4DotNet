using GtkDotNet;
using CsTools.Extensions;

using static System.Console;
using CsTools.Functional;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;
using System.Runtime.InteropServices;

static class Web
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => app
                .SideEffect(_ =>
                {
                    var webkitType = GType.Get(GTypeEnum.WebKitWebView);
                    GType.Ensure(webkitType);
                })
                .SideEffect(app =>
                    Builder.FromDotNetResource("builderWeb").Use(
                        builder =>
                        {
                            builder
                                .GetObject<WindowHandle>("window", w => w
                                    .SetApplication(app)
                                    .Show());
                            builder
                                //.GetObject<WebViewHandle>("webview", wv => wv.LoadUri("https://selqio.com/tools/webrtc-tester"));
                                .GetObject<WebViewHandle>("webview", wv =>
                                {
                                    wv.OnPermissionRequest(rq =>
                                    {
                                        webkit_permission_request_allow(rq);
                                        return true;
                                    });
                                    //wv.LoadUri("https://selqio.com/tools/webrtc-tester");
                                    wv.LoadUri("http://localhost:5173");
                                });
                        })))
            .Run(0, IntPtr.Zero);

    public static int Run1()
        => Application
            .New("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Hello Web View👍")
                    .DefaultSize(800, 600)
                    .Child(
                        WebKit
                            .New()
                            .SideEffect(wk =>
                                wk.AddController(
                                EventControllerKey
                                    .New()
                                    .RefSink()
                                    .OnRawKeyPressed((k, kc, m) =>
                                    {
                                        if (kc == 73)
                                        {
                                            // prevent blink_cb crash!
                                            wk.RunJavascript(
"""
    console.log("Der F7")
    document.dispatchEvent(new KeyboardEvent('keydown', {
        key: "F7",
        code: "F7"
    })) 
""");
                                            GC.Collect();
                                            return true;
                                        }
                                        else
                                            return false;
                                    })))
                            .SideEffect(w => w.GetSettings()
                                .SideEffect(s =>
                                {
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                    WriteLine($"CursiveFontFamily: {s.CursiveFontFamily}");
                                    s.EnableDeveloperExtras = true;
                                    WriteLine($"EnableDevExtras: {s.EnableDeveloperExtras}");
                                }))
                            .OnLoadChanged((w, e) =>
                                e.SideEffectIf(e == WebViewLoad.Finished,
                                    _ => w.RunJavascript("console.log('called from C#')")))
                            .DisableContextMenu()
                            .OnAlert((w, text) =>
                                text
                                    .SideEffectIf(text == "showDevTools",
                                        _ => w.GetInspector().Show())
                                    .SideEffect(text => WriteLine($"on alert: {text}")))
                            .LoadUri($"file://{Directory.GetCurrentDirectory()}/webroot/index.html")
                    )
                    .Show())
            .Run(0, IntPtr.Zero)
            .SideEffect(_ => GC.Collect())
            .SideEffect(_ => GC.Collect());


    [DllImport("libwebkitgtk-6.0.so.4", CallingConvention = CallingConvention.Cdecl)]
    static extern void webkit_permission_request_allow(nint request);
}
