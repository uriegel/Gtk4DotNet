using System.Drawing;
using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// A WebKit Webview. When build from template.ui, you have to call <see cref="Application.WithWebKit"/> before creating the window from template.
/// </summary>
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

    /// <summary>
    /// When this method is called before when WebView is created,a callback will be installed to be called, when alert() is called from javascript.
    /// You can show your own alert dialog, or you can use alert to communicate with the WebView.
    /// </summary>
    /// <param name="alert"></param>
    /// <returns>This instance for function chaining</returns>
    public WebView OnAlert(Action<WebView, string> alert)
        => this.SideEffect(a => SignalConnect<TwoPointerDelegate>("script-dialog",
            (nint _, nint s) => alert(this, ScriptDialogGetMessage(s).PtrToString(false) ?? "")));

    /// <summary>
    /// Disables the default context menu.
    /// </summary>
    /// <returns>This instance for function chaining</returns>
    public WebView DisableContextMenu() => OnContextMenu(_ => true);

    /// <summary>
    /// Installs a callback called when in the WebView a context menu should be shown. Returning false will show the default menu, returning true will prevent it.
    /// </summary>
    /// <param name="contextMenu"></param>
    /// <returns></returns>
    public WebView OnContextMenu(Func<WebView, bool> contextMenu)
        => this.SideEffect(a => SignalConnect<Action>("context-menu", () => contextMenu(this)));

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern new nint Type();

    /// <summary>
    /// Calls a javascript function in the WebView. The action is started as an asyncronous process 
    /// </summary>
    /// <param name="script">The script that is being performed</param>
    public void RunJavascript(string script)
    {
        var key = GtkDelegates.Instance.GetKey("RunJavascript");
        ThreePointerDelegate callback = (_, result, ___) =>
        {
            var res = FinishJavascript(this, result, 0);
            GtkDelegates.Instance.Remove(key.Key);
            if (res != 0 && JscIsString(res))
                Free(res);
        };
        GtkDelegates.Instance.Add(key, callback);
        EvaluateJavascript(this, script, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
    }

    /// <summary>
    /// Gets the <see cref="WebViewSettings"/> for this webview.
    /// </summary>
    /// <returns></returns>
    public WebViewSettings GetSettings()
    {
        var res = GetSettings(this);
        res.AutoDestroyed = true;
        return res;
    }

    /// <summary>
    /// Sets a background color for this WebView
    /// </summary>
    /// <param name="color"></param>
    /// <returns>This instance for function chaining</returns>
    public WebView BackgroundColor(Color color)
    {
        var rgba = GtkRgba.FromColor(color);
        SetBackgroundColor(this, ref rgba);
        return this;
    }

    /// <summary>
    /// Gets the DevTools Inspector for this Webview.
    /// </summary>
    /// <returns></returns>
    public WebInspector GetInspector()
    {
        var insp = GetInspector(this);
        insp.AutoDestroyed = true;
        insp.CheckDiagnostics();
        return insp;
    }

    /// <summary>
    /// Displays the DevTools Inspector for this Webview. It will be shown as a separate window.
    /// </summary>
    public void ShowInspector()
    {
        try
        {
            Gtk.BeginInvoke(200, () =>
            {
                var inspector = GetInspector();
                inspector.Show();
                GrabFocus();
                DetachInspector();

                async void DetachInspector()
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(600));
                    inspector.Detach();
                    inspector.Dispose();
                }
            });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Could not show devtools: {e}");
        }
    }

    public WebView() : base() { }

    public WebView(Builder builder, string? name = null) : base(builder, name) { }

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
    extern static nint FinishJavascript(WebView WebViewHandle, nint result, nint _);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_string", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsString(nint obj);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_undefined", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsUndefined(nint obj);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_inspector", CallingConvention = CallingConvention.Cdecl)]
    extern static WebInspector GetInspector(WebView webView);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_settings", CallingConvention = CallingConvention.Cdecl)]
    extern static WebViewSettings GetSettings(WebView webView);
}


