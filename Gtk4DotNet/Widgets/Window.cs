using System.Drawing;
using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// A Gtk Window. 
/// </summary>
/// <remarks>
/// It is recommended to build a window from a .NET resource template.ui.
/// </remarks>
public class Window : Widget
{
    public string Title
    {
        get => GetTitle(this).PtrToString(false) ?? "";
        set => SetTitle(this, value);
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

    public Size Size
    {
        get
        {
            GetSize(this, out var w, out var h);
            return new(w, h);
        }
    }
    public new int Width { get => Size.Width; }
    public new int Height { get => Size.Height; }

    public void Present() => Present(this);

    public void CloseWindow() => CloseWindow(this);

    public void SetDefaultSize(int width, int height) => SetDefaultSize(this, width, height);

    public void SetChild(Widget child) => SetChild(this, child);

    /// <summary>
    /// Sets a transient parent for the window.
    /// Dialog windows should be set transient for the main application window they were spawned from. 
    /// This allows window managers to e.g. keep the dialog on top of the main window, or center the dialog over the main window. 
    /// Passing null for parent unsets the current transient window.
    /// </summary>
    /// <param name="parent"></param>
    public void TransientFor(Window parent) => TransientFor(this, parent);

    /// <summary>
    /// Installs a callback that is being called when the window is about to close. You can prevent it by returning true in  the callback.
    /// </summary>
    /// <param name="preventClosing"></param>
    public void OnClose(Func<Window, bool> preventClosing)
        => SignalConnect<TwoPointerBoolRetDelegate>("close-request", (_, ___) => preventClosing(this));

    /// <summary>
    /// Installs an asynchronous callback that is being called when the window is about to close. You can prevent it by returning true in  the callback.
    /// </summary>
    /// <param name="preventClosing"></param>
    public void OnClose(Func<Window, Task<bool>> preventClosing)
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

    /// <summary>
    /// Gets the application that this window belongs to.
    /// </summary>
    /// <returns></returns>
    public Application GetApplication()
        => _GetApplication(this).SideEffect(a => a.AutoDestroyed = true);

    public TWidget GetFocus<TWidget>() where TWidget : Widget, new()
    {
        var res = new TWidget();
        res.SetInternalHandle(GetFocus(this));
        AutoDestroyed = true;
        return res;
    }

    /// <summary>
    /// Creates a new Window. This window should be added to the <see cref="Application"/> with the help of <see cref="Application.AddWindow(Window)"/> 
    /// </summary>
    /// <returns>A newly created Window</returns>
    public static Window New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }

    public Window(Builder builder, string? name = null) : base(builder, name) { }

    public Window(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    public Window() : base() { }

    protected void Construct()
    {
        SetInternalHandle(_New().GetInternalHandle());
        CheckDiagnostics();
    }

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

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_default_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetSize(Window window, out int width, out int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Window _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_present", CallingConvention = CallingConvention.Cdecl)]
    extern static void Present(Window window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_transient_for", CallingConvention = CallingConvention.Cdecl)]
    extern static void TransientFor(Window window, Window parent);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_focus", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint GetFocus(Window window);

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

    public static THandle Closing<THandle>(this THandle window, Func<Window, Task<bool>> preventClosing)
        where THandle : Window
        => window.SideEffect(a => window.OnClose((Window win) => preventClosing(win)));
}


