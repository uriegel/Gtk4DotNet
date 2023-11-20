using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Window
{
    public static WindowHandle Title(this WindowHandle window, string title)
        => window.SideEffect(w => SetTitle(window, title));

    public static WindowHandle DefaultSize(this WindowHandle window, int width, int height)
        => window.SideEffect(w => SetDefaultSize(window, width, height));

    public static WindowHandle Child(this WindowHandle window, WidgetHandle child)
        => window.SideEffect(w => SetChild(window, child));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WindowHandle New(WindowType windowType);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_move", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Move(this WindowHandle window, int x, int y);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_close", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Close(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_modal", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetModal(this WindowHandle window, bool set);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_maximize", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Maximize(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_is_maximized", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsMaximized(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_transient_for", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTransientFor(this WindowHandle window, IntPtr parent);

     [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_application", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetApplication(this WindowHandle window, ApplicationHandle application);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetIconName(this WindowHandle window, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_size", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GetSize(WindowHandle window, out int width, out int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_position", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GetPosition(WindowHandle window, out int x, out int y);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this WindowHandle window, WidgetHandle child);
   
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_title", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitle(this WindowHandle window, string title);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_default_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDefaultSize(this WindowHandle window, int width, int height);

}