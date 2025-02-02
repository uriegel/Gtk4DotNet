using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class WebInspector
{
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_inspector_show", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Show(this WebInspectorHandle inspector);
    
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_inspector_detach", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Detach(this WebInspectorHandle inspector);
}
