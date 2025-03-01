using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ApplicationWindow
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ApplicationWindowHandle New(ApplicationHandle application);

    // TODO Transfer this to WebWindowNetCore
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();
}
