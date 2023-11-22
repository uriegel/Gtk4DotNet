using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

// TODO InspectorShow
// TODO ScriptDialogGetMessage
// TODO RunJavascript
// TODO Application.EnableSynchronizationContext();
// TODO SetIconFromDotNetResource
// TODO Gtk.SignalConnect<TwoIntPtr>(webView, "load-changed", (_, e) =>
// TODO Gtk.SignalConnect<TwoIntPtr>(webView, "script-dialog", (_, d) =>
// TODO Window.SetDefaultSize(
// TODO Window.Maximize(window);
public static class WebView
{
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewHandle New();

    public static WebViewHandle LoadUri(this WebViewHandle webView, string uri)
        => webView.SideEffect(w => w._LoadUri(uri));

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_script_dialog_get_message", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr ScriptDialogGetMessage(this WebViewHandle webView);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_load_uri", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LoadUri(this WebViewHandle webView, string uri);

    // public static void RunJavascript(this WebViewHandle webView, string script)
    // {
    //     var key = Delegates.GetKey();
    //     ThreeIntPtr callback = (_, result, ___) =>
    //     {
    //         var res = FinishJavascript(webView, result, IntPtr.Zero);
    //         Delegates.Remove(key);
    //         if (res != IntPtr.Zero && JscIsString(res))
    //             GObject.Free(res);
    //     };
    //     Delegates.Add(key, callback);
    //     EvaluateJavascript(webView, script, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
    // }
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_settings", CallingConvention = CallingConvention.Cdecl)]
    public extern static WebViewSettingsHandle GetSettings(this WebViewHandle webView);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_get_inspector", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetInspector(this WebViewHandle webView);

    // [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_inspector_show", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void InspectorShow(this  inspector);

    // [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void EvaluateJavascript(this IntPtr webView, string script, int _, IntPtr __, IntPtr ___, IntPtr ____, IntPtr callback, IntPtr _____);

    // [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript_finish", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr FinishJavascript(this IntPtr webView, IntPtr result, IntPtr _);

    // [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_string", CallingConvention = CallingConvention.Cdecl)]
    // extern static bool JscIsString(IntPtr obj);

    // [DllImport(Libs.LibWebKit, EntryPoint = "jsc_value_is_undefined", CallingConvention = CallingConvention.Cdecl)]
    // extern static bool JscIsUndefined(IntPtr obj);
}
