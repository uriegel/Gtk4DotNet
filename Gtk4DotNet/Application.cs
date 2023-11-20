using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Application
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ApplicationHandle New(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WindowHandle NewWindow(this ApplicationHandle app);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_add_window", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddWindow(this ApplicationHandle app, IntPtr window);

    [DllImport(Libs.LibGtk, EntryPoint="g_application_quit", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Quit(this ApplicationHandle app);

    [DllImport(Libs.LibGtk, EntryPoint="g_application_run", CallingConvention = CallingConvention.Cdecl)]
    public extern static int Run(this ApplicationHandle app, int c, IntPtr a);

    public static ApplicationHandle OnActivate(this ApplicationHandle app, Action<ApplicationHandle> activate)
    {
        void onActivate(IntPtr _)  => activate(app);
        return app.SideEffect(a => Gtk.SignalConnect(a, "activate", Marshal.GetFunctionPointerForDelegate((OnActivateDelegate)onActivate), IntPtr.Zero, IntPtr.Zero, 0));
    }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_application_set_accels_for_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetAccelsForAction(ApplicationHandle app, string action, [In] string[] accels);

    [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewAction(string action, string p);

    [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewStatefulAction(string action, string p, IntPtr state);

    [DllImport(Libs.LibGtk, EntryPoint="g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewBool(int value);

    [DllImport(Libs.LibGtk, EntryPoint="g_variant_new_int32", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewInt(int value);

    [DllImport(Libs.LibGtk, EntryPoint="g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewString(string value);

    [DllImport(Libs.LibGtk, EntryPoint="g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddAction(ApplicationHandle app, IntPtr action);
    
    [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_set_enabled", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EnableAction(IntPtr action, int enabled);
}
