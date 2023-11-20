using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Gtk
{
    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnect(this GtkHandle widget, string name, IntPtr callback, IntPtr obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_data", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnect(this GtkHandle widget, string name, IntPtr callback, IntPtr n, IntPtr n2, int n3);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Main();

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main_quit", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void MainQuit();

    // [DllImport(Libs.LibGtk, EntryPoint="g_idle_add_full", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void IdleAddFull(int priority, IntPtr func, IntPtr nil, IntPtr nil2);

    // [DllImport(Libs.LibGtk, EntryPoint="g_content_type_guess", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr GuessContentType(string filename, IntPtr nil1,  IntPtr nil2, IntPtr nil3);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_init", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Init (ref int argc, ref IntPtr argv);
   
    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_data", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnectAfter(this GtkHandle widget, string name, IntPtr callback, IntPtr n, IntPtr n2, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_handler_disconnect", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SignalDisconnect(this GtkHandle widget, long id);
}

