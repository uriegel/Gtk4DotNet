using System.Reflection;
using System.Runtime.InteropServices;
using CsTools;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Window
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WindowHandle New(WindowType windowType);

    public static WindowHandle Title(this WindowHandle window, string title)
        => window.SideEffect(w => SetTitle(window, title));

    public static WindowHandle SetApplication(this WindowHandle window, ApplicationHandle application)
        => window.SideEffect(w => w._SetApplication(application));

    public static WindowHandle ResourceIcon(this WindowHandle window, string resourceIconPath)
    {
        var themeDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Gtk4DotNet", 
            Assembly
                .GetCallingAssembly()
                .GetName()
                .Name!);
        var iconDir = Path
                        .Combine(themeDir, "hicolor", "48x48", "apps")
                        .EnsureDirectoryExists();
        var resIcon = Assembly
            .GetEntryAssembly()
            ?.GetManifestResourceStream(resourceIconPath);
        using var iconFile = File.OpenWrite(Path.Combine(iconDir, "icon.png"));
        resIcon?.CopyTo(iconFile);

        window
            .GetDisplay()
            .GetIconTheme()
            .AddSearchPath(themeDir); 
        window.SetIconName("icon");
        return window;
    }

    public static WindowHandle DefaultSize(this WindowHandle window, int width, int height)
        => window.SideEffect(w => SetDefaultSize(window, width, height));

    public static WindowHandle Child(this WindowHandle window, WidgetHandle child)
        => window.SideEffect(w => SetChild(window, child));

    public static WindowHandle OnClose(this WindowHandle window, Func<WindowHandle, bool> closing)
    {
        bool onClose(IntPtr _, IntPtr __)  => closing(window);
        return window.SideEffect(a => Gtk.SignalConnect(a, "close-request", Marshal.GetFunctionPointerForDelegate((TwoPointerBoolRetDelegate)onClose), IntPtr.Zero, IntPtr.Zero, 0));
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_move", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Move(this WindowHandle window, int x, int y);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_close", CallingConvention = CallingConvention.Cdecl)]
    public extern static void CloseWindow(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_modal", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetModal(this WindowHandle window, bool set);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_maximize", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Maximize(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_is_maximized", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsMaximized(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_transient_for", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTransientFor(this WindowHandle window, IntPtr parent);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_application", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetApplication(this WindowHandle window, ApplicationHandle application);

    public static WindowHandle IconName(this WindowHandle window, string name)
        => window.SideEffect(w => w.SetIconName(name));

    public static (int, int) GetSize(this WindowHandle window)
    {
        window.GetSize(out int width, out int height);
        return (width, height);
    }

    public static int GetWidth(this WindowHandle window)
        => window.GetSize().Item1;

    public static int GetHeight(this WindowHandle window)
        => window.GetSize().Item2;

    public static (int, int) GetPosition(this WindowHandle window)
    {
        window.GetPosition(out var x, out var y);
        return (x, y);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this WindowHandle window, WidgetHandle child);
   
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_title", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitle(this WindowHandle window, string title);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_default_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDefaultSize(this WindowHandle window, int width, int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(this WindowHandle window, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetSize(this WindowHandle window, out int width, out int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_position", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetPosition(this WindowHandle window, out int x, out int y);
   
}