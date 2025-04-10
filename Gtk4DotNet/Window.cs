using System.Reflection;
using System.Runtime.InteropServices;
using CsTools;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Window
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static WindowHandle New(WindowType windowType);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static THandle Title<THandle>(this THandle window, string title)
        where THandle : WindowHandle
        => window.SideEffect(w => SetTitle(window, title));

    public static THandle Titlebar<THandle>(this THandle window, WidgetHandle titlebar)
        where THandle : WindowHandle
        => window.SideEffect(w => SetTitlebar(window, titlebar));

    public static THandle SetApplication<THandle>(this THandle window, ApplicationHandle application)
        where THandle : WindowHandle
        => window.SideEffect(w => w._SetApplication(application));

    public static ApplicationHandle GetApplication(this WindowHandle window)
        => window._GetApplication().SideEffect(a => a.IsFloating = true);

    public static THandle TransientFor<THandle>(this THandle window, WindowHandle parent)
        where THandle : WindowHandle
        => window.SideEffect(w => w.SetTransientFor(parent));
    public static THandle Modal<THandle>(this THandle window)
        where THandle : WindowHandle
        => window.SideEffect(w => w.SetModal(true));

    public static THandle Resizable<THandle>(this THandle window, bool set)
        where THandle : WindowHandle
        => window.SideEffect(w => w.SetResizable(set));

    public static THandle NotDecorated<THandle>(this THandle window)
        where THandle : WindowHandle
        => window.SideEffect(w => w._SetDecorated(false));

    [Obsolete("Icon per window is now deprecated in GTK4, especially with Wayland", true)]
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

    public static THandle DefaultSize<THandle>(this THandle window, int width, int height)
        where THandle : WindowHandle
        => window.SideEffect(w => SetDefaultSize(window, width, height));

    public static THandle Child<THandle>(this THandle window, WidgetHandle child)
        where THandle : WindowHandle
        => window.SideEffect(w => SetChild(window, child));

    public static THandle GetChild<THandle>(this WindowHandle window)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(_GetChild(window).GetInternalHandle());
        return res;
    }

    public static THandle OnClose<THandle>(this THandle window, Func<WindowHandle, bool> preventClosing)
        where THandle : WindowHandle
        => window.SideEffect(a => Gtk.SignalConnect<TwoPointerBoolRetDelegate>(a, "close-request", (_, ___) => preventClosing(window)));

    public static THandle GetFocus<THandle>(this WindowHandle window)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(window.GetFocus());
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_focus", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint GetFocus(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_move", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Move(this WindowHandle window, int x, int y);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_close", CallingConvention = CallingConvention.Cdecl)]
    public extern static void CloseWindow(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_modal", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModal(this WindowHandle window, bool set);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_maximize", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Maximize(this WindowHandle window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_is_maximized", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsMaximized(this WindowHandle window);

    [Obsolete("Icon per window is now deprecated in GTK4, especially with Wayland", true)]
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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_transient_for", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTransientFor(this WindowHandle window, WindowHandle parent);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_application", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetApplication(this WindowHandle window, ApplicationHandle application);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this WindowHandle window, WidgetHandle child);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_titlebar", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitlebar(this WindowHandle window, WidgetHandle titlebar);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_title", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitle(this WindowHandle window, string title);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_default_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDefaultSize(this WindowHandle window, int width, int height);

    [DllImport(Libs.LibGtk, CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_window_get_child")]
    static extern WidgetHandle _GetChild(this WindowHandle window);

    [Obsolete("Icon per window is now deprecated in GTK4, especially with Wayland", true)]
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(this WindowHandle window, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_default_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetSize(this WindowHandle window, out int width, out int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_position", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetPosition(this WindowHandle window, out int x, out int y);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_resizable", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetResizable(this WindowHandle window, bool set);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_decorated", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetDecorated(this WindowHandle window, bool set);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_application", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationHandle _GetApplication(this WindowHandle window);
}