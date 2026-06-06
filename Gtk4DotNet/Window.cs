using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class Window : Widget
{
    public string? Title
    {
        get => GetTitle(this).PtrToString(false);
        set => SetTitle(this, value ?? "");
    }

    public bool IsMaximized
    {
        get => GetIsMaximized(this);
        set 
        {
            if (value)
                Maximize(this);
            else
                UnMaximize(this);
        }
    }

    public Window() : base() { }
    public Window(nint obj) : base() => SetInternalHandle(obj);

    public void SetDefaultSize(int width, int height) => SetDefaultSize(this, width, height);

    public void SetChild(Widget child) => SetChild(this, child);

    internal Window(Widget widget) : base() => handle = widget.TakeHandle();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_title", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitle(Window window, string title);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_title", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetTitle(Window window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_default_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDefaultSize(Window window, int width, int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(Window window, Widget child);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_is_maximized", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetIsMaximized(Window window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_maximize", CallingConvention = CallingConvention.Cdecl)]
    extern static void Maximize(Window window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_unmaximize", CallingConvention = CallingConvention.Cdecl)]
    extern static void UnMaximize(Window window);
}

public static class WindowExtensions
{
    public static THandle Title<THandle>(this THandle window, string title)
            where THandle : Window
            => window.SideEffect(w => w.Title = title);

    public static THandle DefaultSize<THandle>(this THandle window, int width, int height)
        where THandle : Window
        => window.SideEffect(w => w.SetDefaultSize(width, height));

    public static THandle Child<THandle>(this THandle window, Widget child)
        where THandle : Window
        => window.SideEffect(w => w.SetChild(child));
}


