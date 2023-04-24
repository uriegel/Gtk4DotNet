using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Window
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(WindowType windowType);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_default_size", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetDefaultSize(this IntPtr window, int width, int height);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_move", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Move(this IntPtr window, int x, int y);

    public static (int, int) GetPosition(this IntPtr window)
    {
        GetPosition(window, out var x, out var y);
        return (x, y);
    }

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_close", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Close(this IntPtr window);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_modal", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetModal(this IntPtr window, bool set);
        
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_title", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTitle(this IntPtr window, string title);

    public static (int, int) GetSize(this IntPtr window)
    {
        GetSize(window, out var w, out var h);
        return (w, h);
    }        

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_maximize", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Maximize(this IntPtr window);        

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_is_maximized", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsMaximized(this IntPtr window);        
    
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_transient_for", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTransientFor(this IntPtr window, IntPtr parent);  

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool SetChild(this IntPtr window, IntPtr child);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_application", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetApplication(this IntPtr window, IntPtr application);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetIconName(this IntPtr window, string name);  

    /// <summary>
    /// Sets the window icon. It uses an icon contained as DotNet resource
    /// </summary>
    /// <param name="window"></param>
    /// <param name="resourceIconPath">DotNet resource path of the icon</param>
    public static void SetIconFromDotNetResource(IntPtr window, string resourceIconPath)
    {
        var themeDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Gtk4DotNet", 
                    Assembly
                        .GetCallingAssembly()
                        .GetName()
                        .Name);
        var iconDir = Path.Combine(themeDir, "hicolor", "48x48", "apps");
        Directory.CreateDirectory(iconDir);

        var resIcon = System.Reflection.Assembly
            .GetEntryAssembly()
            ?.GetManifestResourceStream(resourceIconPath);
        using var iconFile = File.OpenWrite(Path.Combine(iconDir, "icon.png"));
        resIcon.CopyTo(iconFile);

        var theme = Display.IconThemeForDisplay(Widget.GetDisplay(window));
        IconTheme.AddSearchPath(theme, themeDir); 
        window.SetIconName("icon");
    }

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_get_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetSize(IntPtr window, out int width, out int height);  

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_window_get_position", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetPosition(IntPtr window, out int x, out int y);
}



