using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class AdwDialog : Widget
{
    public void Present(Widget parent) => Present(this, parent);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_present", CallingConvention = CallingConvention.Cdecl)]
    extern static void Present(AdwDialog dialog, Widget parent);
}
