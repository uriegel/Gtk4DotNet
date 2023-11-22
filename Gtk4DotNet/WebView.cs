using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class WebView
{
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewHandle New();

    public static WebViewHandle LoadUri(this WebViewHandle webView, string uri)
        => webView.SideEffect(w => w._LoadUri(uri));

    public static WebViewHandle OnLoadChanged(this WebViewHandle webView, Action<WebViewHandle, WebViewLoad> loadChanged)
    {
        void onClick(IntPtr _, IntPtr e)  => loadChanged(webView, (WebViewLoad)e);
        return webView.SideEffect(a => Gtk.SignalConnect(a, "load-changed", Marshal.GetFunctionPointerForDelegate((TwoPointerDelegate)onClick), IntPtr.Zero, IntPtr.Zero, 0));
    }

    public static WebViewHandle OnAlert(this WebViewHandle webView, Action<WebViewHandle, string?> alert)
    {
        void onAlert(IntPtr _, IntPtr s) => alert(webView, Marshal.PtrToStringUTF8(ScriptDialogGetMessage(s)));
        return webView.SideEffect(a => Gtk.SignalConnect(a, "script-dialog", Marshal.GetFunctionPointerForDelegate((TwoPointerDelegate)onAlert), IntPtr.Zero, IntPtr.Zero, 0));
    }

    public static WebViewHandle DisableContextMenu(this WebViewHandle webView)
        => webView.OnContextMenu(_ => true);

    public static WebViewHandle OnContextMenu(this WebViewHandle webView, Func<WebViewHandle, bool> contextMenu)
    {
        bool onContextMenu() => contextMenu(webView);
        return webView.SideEffect(a => Gtk.SignalConnect(a, "context-menu", Marshal.GetFunctionPointerForDelegate((BoolRetDelegate)onContextMenu), IntPtr.Zero, IntPtr.Zero, 0));
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_script_dialog_get_message", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr ScriptDialogGetMessage(IntPtr msg);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_load_uri", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadUri(this WebViewHandle webView, string uri);

    public static void RunJavascript(this WebViewHandle webView, string script)
    {
        var key = Delegates.GetKey();
        ThreePointerDelegate callback = (_, result, ___) =>
        {
            var res = FinishJavascript(webView, result, IntPtr.Zero);
            Delegates.Remove(key);
            if (res != IntPtr.Zero && JscIsString(res))
                GObject.Free(res);
        };
        Delegates.Add(key, callback);
        EvaluateJavascript(webView, script, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
    }

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_settings", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewSettingsHandle GetSettings(this WebViewHandle webView);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_inspector", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebInspectorHandle GetInspector(this WebViewHandle webView);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript", CallingConvention = CallingConvention.Cdecl)]
    extern static void EvaluateJavascript(this WebViewHandle webView, string script, int _, IntPtr __, IntPtr ___, IntPtr ____, IntPtr callback, IntPtr _____);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr FinishJavascript(this WebViewHandle WebViewHandle, IntPtr result, IntPtr _);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_string", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsString(IntPtr obj);

    [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_undefined", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsUndefined(IntPtr obj);
}
