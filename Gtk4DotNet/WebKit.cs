using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;
using System.Drawing;

namespace GtkDotNet;

public static class WebKit
{
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewHandle New();

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_new_with_context", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewHandle New(WebKitWebContextHandle c);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();        
    
    public static WebViewHandle LoadUri(this WebViewHandle webView, string uri)
        => webView.SideEffect(w => w._LoadUri(uri));

    public static WebViewHandle OnLoadChanged(this WebViewHandle webView, Action<WebViewHandle, WebViewLoad> loadChanged)
        => webView.SideEffect(a => Gtk.SignalConnect<TwoPointerDelegate>(a, "load-changed", 
            (IntPtr _, IntPtr e)  => loadChanged(webView, (WebViewLoad)e)));

    public static WebViewHandle OnAlert(this WebViewHandle webView, Action<WebViewHandle, string?> alert)
        => webView.SideEffect(a => Gtk.SignalConnect<TwoPointerDelegate>(a, "script-dialog", 
            (IntPtr _, IntPtr s) => alert(webView, Marshal.PtrToStringUTF8(ScriptDialogGetMessage(s)))));

    public static WebViewHandle DisableContextMenu(this WebViewHandle webView)
        => webView.OnContextMenu(_ => true);

    public static WebViewHandle OnContextMenu(this WebViewHandle webView, Func<WebViewHandle, bool> contextMenu)
        => webView.SideEffect(a => Gtk.SignalConnect<Action>(a, "context-menu", () => contextMenu(webView)));

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_script_dialog_get_message", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr ScriptDialogGetMessage(IntPtr msg);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_load_uri", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadUri(this WebViewHandle webView, string uri);

    public static void RunJavascript(this WebViewHandle webView, string script)
    {
        var key = GtkDelegates.GetKey();
        ThreePointerDelegate callback = (_, result, ___) =>
        {
            var res = FinishJavascript(webView, result, IntPtr.Zero);
            GtkDelegates.Remove(key);
            if (res != IntPtr.Zero && JscIsString(res))
                GObject.Free(res);
        };
        GtkDelegates.Add(key, callback);
        EvaluateJavascript(webView, script, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_settings", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewSettingsHandle GetSettings(this WebViewHandle webView);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_inspector", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebInspectorHandle GetInspector(this WebViewHandle webView);

    public static WebViewHandle BackgroundColor(this WebViewHandle webView, Color color)
    {
        var rgba = GtkRgba.FromColor(color);
        webView.SetBackgroundColor(ref rgba);
        return webView;
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_set_background_color", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetBackgroundColor(this WebViewHandle webView,  ref GtkRgba rgba);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript", CallingConvention = CallingConvention.Cdecl)]
    extern static void EvaluateJavascript(this WebViewHandle webView, string script, int _, IntPtr __, IntPtr ___, IntPtr ____, IntPtr callback, IntPtr _____);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr FinishJavascript(this WebViewHandle WebViewHandle, IntPtr result, IntPtr _);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_string", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsString(IntPtr obj);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_undefined", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsUndefined(IntPtr obj);
}
