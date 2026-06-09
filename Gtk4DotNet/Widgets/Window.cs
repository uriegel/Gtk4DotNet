using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

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

    public void CloseWindow() => CloseWindow(this);

    public void SetDefaultSize(int width, int height) => SetDefaultSize(this, width, height);

    public void SetChild(Widget child) => SetChild(this, child);

    public void OnClose(Func<Window, bool> preventClosing)
        => SignalConnect<TwoPointerBoolRetDelegate>("close-request", (_, ___) => preventClosing(this));

    public void OnCloseAsync(Func<Window, Task<bool>> preventClosing)
        => SignalConnect<TwoPointerBoolRetDelegate>("close-request", (_, ___) =>
        {
            if (forceClose)
                return false;
            Run();
            return true;

            async void Run()
            {
                var ret = await preventClosing(this);
                if (!ret)
                {
                    forceClose = true;
                    CloseWindow();
                }
            }
        });

    public Application GetApplication()
        => _GetApplication(this).SideEffect(a => a.IsFloating = true);

    public Window(Builder builder, string? name = null) : base(builder, name) { }

    public Window() : base() { }
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_application", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SetApplication(Window window, Application application);

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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_close", CallingConvention = CallingConvention.Cdecl)]
    extern static void CloseWindow(Window window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_application", CallingConvention = CallingConvention.Cdecl)]
    extern static Application _GetApplication(Window window);

    bool forceClose;
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

    public static THandle Closing<THandle>(this THandle window, Func<Window, bool> preventClosing)
        where THandle : Window
        => window.SideEffect(a => window.OnClose((Window win) => preventClosing(win)));

    public static THandle ClosingAsync<THandle>(this THandle window, Func<Window, Task<bool>> preventClosing)
        where THandle : Window
        => window.SideEffect(a => window.OnCloseAsync((Window win) => preventClosing(win)));
}


