using System.Drawing;
using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class WebView : Widget
{
    public static WebView New()
    {
        var webview = _New();
        webview.CheckDiagnostics();
        return webview;
    }

    public WebView LoadUri(string uri)
        => this.SideEffect(w => LoadUri(this, uri));

    public WebView OnLoadChanged(Action<WebView, WebViewLoad> loadChanged)
        => this.SideEffect(a => SignalConnect<TwoPointerDelegate>("load-changed", (nint _, nint e) => loadChanged(this, (WebViewLoad)e)));

    public WebView OnAlert(Action<WebView, string?> alert)
        => this.SideEffect(a => SignalConnect<TwoPointerDelegate>("script-dialog",
            (IntPtr _, IntPtr s) => alert(this, Marshal.PtrToStringUTF8(ScriptDialogGetMessage(s)))));

    // public WebView OnPermissionRequest(Func<nint, bool> permissionRequest)
    //     => this.SideEffect(a => Gtk.SignalConnect<ThreePointerBoolRetDelegate>(a, "permission-request", 
    //         (IntPtr _, IntPtr rq, IntPtr _)  => permissionRequest(rq)));

    public WebView DisableContextMenu() => OnContextMenu(_ => true);

    public WebView OnContextMenu(Func<WebView, bool> contextMenu)
        => this.SideEffect(a => SignalConnect<Action>("context-menu", () => contextMenu(this)));

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern new GType Type();

    public static void RunJavascript(WebView webView, string script)
    {
        var key = GtkDelegates.GetKey();
        ThreePointerDelegate callback = (_, result, ___) =>
        {
            var res = FinishJavascript(webView, result, IntPtr.Zero);
            GtkDelegates.Remove(key);
            if (res != IntPtr.Zero && JscIsString(res))
                Free(res);
        };
        GtkDelegates.Add(key, callback);
        EvaluateJavascript(webView, script, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
    }

    public WebViewSettings GetSettings() => GetSettings(this);

    public WebView BackgroundColor(Color color)
    {
        var rgba = GtkRgba.FromColor(color);
        SetBackgroundColor(this, ref rgba);
        return this;
    }

    public WebInspector GetInspector()
    {
        var insp = GetInspector(this);
        insp.CheckDiagnostics();
        return insp;
    }

    public WebView() : base() { }

    public WebView(Builder builder, string? name = null) : base(builder, name) { }

    // [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_new_with_context", CallingConvention = CallingConvention.Cdecl)]
    // public extern static WebView _New(WebKitWebContextHandle c);

    // [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    // public static extern GTypeHandle Type();        

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_new", CallingConvention = CallingConvention.Cdecl)]
    extern static WebView _New();

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_load_uri", CallingConvention = CallingConvention.Cdecl)]
    extern static void LoadUri(WebView webView, string uri);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_script_dialog_get_message", CallingConvention = CallingConvention.Cdecl)]
    extern static nint ScriptDialogGetMessage(nint msg);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_set_background_color", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBackgroundColor(WebView webView, ref GtkRgba rgba);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript", CallingConvention = CallingConvention.Cdecl)]
    extern static void EvaluateJavascript(WebView webView, string script, int _, nint __, nint ___, nint ____, nint callback, nint _____);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr FinishJavascript(WebView WebViewHandle, nint result, nint _);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_string", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsString(IntPtr obj);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_undefined", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsUndefined(IntPtr obj);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_inspector", CallingConvention = CallingConvention.Cdecl)]
    extern static WebInspector GetInspector(WebView webView);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_settings", CallingConvention = CallingConvention.Cdecl)]
    extern static WebViewSettings GetSettings(WebView webView);
}

