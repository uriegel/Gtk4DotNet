using System;
using System.Runtime.InteropServices;

namespace GtkDotNet
{
    public class WebKit
    {
        [DllImport(Globals.LibWebKit, EntryPoint="webkit_web_view_new", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr New();

        [DllImport(Globals.LibWebKit, EntryPoint="webkit_web_view_load_uri", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr LoadUri(IntPtr webView, string uri);

        [DllImport(Globals.LibWebKit, EntryPoint="webkit_script_dialog_get_message", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr ScriptDialogGetMessage(IntPtr dialog);

        public static IntPtr RunJavascript(IntPtr webView, string script)
            => RunJavascript(webView, script, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

        [DllImport(Globals.LibWebKit, EntryPoint="webkit_web_view_get_settings", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr GetSettings(IntPtr webView);

        [DllImport(Globals.LibWebKit, EntryPoint="webkit_web_view_get_inspector", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr GetInspector(IntPtr webView);

        [DllImport(Globals.LibWebKit, EntryPoint="webkit_web_inspector_show", CallingConvention = CallingConvention.Cdecl)]
        public extern static void InspectorShow(IntPtr inspector);


        [DllImport(Globals.LibWebKit, EntryPoint="webkit_web_view_run_javascript", CallingConvention = CallingConvention.Cdecl)]
        extern static IntPtr RunJavascript(IntPtr webView, string script, IntPtr nil1, IntPtr nil2, IntPtr nil3);
    }
}