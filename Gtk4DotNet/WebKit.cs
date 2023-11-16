using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

namespace GtkDotNet;
public static class WebKit
{
    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_web_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_web_view_load_uri", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr LoadUri(this IntPtr webView, string uri);

    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_script_dialog_get_message", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr ScriptDialogGetMessage(this IntPtr dialog);

    public static void RunJavascript(this IntPtr webView, string script)
    {
        var key = Delegates.GetKey();
        ThreeIntPtr callback = (_, result, ___) =>
        {
            var res = FinishJavascript(webView, result, IntPtr.Zero);
            Delegates.Remove(key);
            if (res != IntPtr.Zero && JscIsString(res))
                GObject.Free(res);
        };
        Delegates.Add(key, callback);
        EvaluateJavascript(webView, script, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
    }

    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_web_view_get_settings", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetSettings(this IntPtr webView);

    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_web_view_get_inspector", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetInspector(this IntPtr webView);

    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_web_inspector_show", CallingConvention = CallingConvention.Cdecl)]
    public extern static void InspectorShow(this IntPtr inspector);

    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EvaluateJavascript(this IntPtr webView, string script, int _, IntPtr __, IntPtr ___, IntPtr ____, IntPtr callback, IntPtr _____);

    [DllImport(Globals.LibWebKit, EntryPoint = "webkit_web_view_evaluate_javascript_finish", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr FinishJavascript(this IntPtr webView, IntPtr result, IntPtr _);

    [DllImport(Globals.LibWebKit, EntryPoint = "jsc_value_is_string", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsString(IntPtr obj);

    [DllImport(Globals.LibWebKit, EntryPoint = "jsc_value_is_undefined", CallingConvention = CallingConvention.Cdecl)]
    extern static bool JscIsUndefined(IntPtr obj);
}
