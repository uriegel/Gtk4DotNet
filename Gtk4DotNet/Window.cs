using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Window
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(WindowType windowType);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_default_size", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetDefaultSize(IntPtr window, int width, int height);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_move", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Move(IntPtr window, int x, int y);

    public static (int, int) GetPosition(IntPtr window)
    {
        GetPosition(window, out var x, out var y);
        return (x, y);
    }

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_close", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Close(IntPtr window);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_modal", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetModal(IntPtr window, bool set);
        
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_title", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTitle(IntPtr window, string title);

    public static (int, int) GetSize(IntPtr window)
    {
        GetSize(window, out var w, out var h);
        return (w, h);
    }        

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_maximize", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Maximize(IntPtr window);        

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_is_maximized", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsMaximized(IntPtr window);        
    
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_transient_for", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTransientFor(IntPtr window, IntPtr parent);  

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetChild(IntPtr window, IntPtr child);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_application", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetApplication(IntPtr window, IntPtr application);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_get_size", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GetSize(IntPtr window, out int width, out int height);  

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_get_position", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GetPosition(IntPtr window, out int x, out int y);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetIconName(IntPtr window, string name);  
}



