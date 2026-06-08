using System.Runtime.InteropServices;
using Gtk4DotNet;

public class WebInspector : FloatingObject
{
    public void Show() => Show(this);

    public void Detach() => Detach(this);

    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_inspector_show", CallingConvention = CallingConvention.Cdecl)]
    extern static void Show(WebInspector inspector);
    
    [DllImport(Libs.LibWebKit, EntryPoint = "webkit_web_inspector_detach", CallingConvention = CallingConvention.Cdecl)]
    extern static void Detach(WebInspector inspector);
}