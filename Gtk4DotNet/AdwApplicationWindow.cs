using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class AdwApplicationWindow
{
    [DllImport(Libs.LibAdw, EntryPoint = "adw_application_window_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();
}
