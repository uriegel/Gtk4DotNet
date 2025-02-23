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

    public static WindowHandle Title(this WindowHandle window, string title)
        => window.SideEffect(w => SetTitle(window, title));

    public static WindowHandle Titlebar(this WindowHandle window, WidgetHandle titlebar)
        => window.SideEffect(w => SetTitlebar(window, titlebar));

    public static WindowHandle SetApplication(this WindowHandle window, ApplicationHandle application)
        => window.SideEffect(w => w._SetApplication(application));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_application", CallingConvention = CallingConvention.Cdecl)]
    public extern static ApplicationHandle GetApplication(this WindowHandle window);

    public static WindowHandle TransientFor(this WindowHandle window, WindowHandle parent)
        => window.SideEffect(w => w.SetTransientFor(parent));
    public static WindowHandle Modal(this WindowHandle window)
        => window.SideEffect(w => w.SetModal(true));

    public static WindowHandle Resizable(this WindowHandle window, bool set)
        => window.SideEffect(w => w.SetResizable(set));

    public static WindowHandle NotDecorated(this WindowHandle window)
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

    public static WindowHandle DefaultSize(this WindowHandle window, int width, int height)
        => window.SideEffect(w => SetDefaultSize(window, width, height));

    public static WindowHandle Child(this WindowHandle window, WidgetHandle child)
        => window.SideEffect(w => SetChild(window, child));

    [DllImport("libgtk-4.so.1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "gtk_window_get_child")]
    public static extern WidgetHandle GetChild(this WindowHandle window);

    public static WindowHandle OnRealize(this WindowHandle window, Action<WindowHandle> realized)
        => window.SideEffect(a => Gtk.SignalConnect<TwoPointerDelegate>(a, "realize", (_, ___) => realized(window)));

    public static WindowHandle OnClose(this WindowHandle window, Func<WindowHandle, bool> preventClosing)
        => window.SideEffect(a => Gtk.SignalConnect<TwoPointerBoolRetDelegate>(a, "close-request", (_, ___) => preventClosing(window)));

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

    public static WindowHandle AddActions(this WindowHandle win, IEnumerable<GtkAction> actions)
    {
        var gtkActions = actions.OfType<GtkAction>();
        foreach (var action in gtkActions)
        {
            if (action.Action != null)
            {
                // TODO DO actions have to be freed?
                var simpleAction = NewAction(action.Name, null);
                action.action = simpleAction;
                GtkDelegates.Add(action.Action);
                Gtk.SignalConnectAction(simpleAction, "activate", Marshal.GetFunctionPointerForDelegate(action.Action as Delegate), IntPtr.Zero, 0);
                AddAction(win, simpleAction);
            }
            else
            {
                GtkDelegates.Add(action.StateChanged);
                var state = action.StateParameterType == "s"
                    ? Application.NewString(action.State as string ?? "")
                    : Application.NewBool((bool?)action.State == true ? -1 : 0);
                var simpleAction = NewStatefulAction(action.Name, action.StateParameterType, state);
                action.action = simpleAction;
                Gtk.SignalConnectAction(simpleAction, "change-state", Marshal.GetFunctionPointerForDelegate(action.StateChanged), IntPtr.Zero, 0);
                AddAction(win, simpleAction);
            }
        }

        var app = win.GetApplication();
        if (!app.IsInvalid)
        {
            var accelEntries =
                actions
                .Where(n => n.Accelerator != null)
                .Select(n => new { Name = "win." + n.Name, n.Accelerator });
            foreach (var accelEntry in accelEntries)
                Application.SetAccelsForAction(app, accelEntry.Name, [accelEntry.Accelerator, null]);
        }
        else
            Console.Error.WriteLine("Could not get application from window, so I could not attach the accelerators");

        return win;
    }

    public static WindowHandle AddAction(this WindowHandle window, ActionHandle action)
        => window.SideEffect(w => w._AddAction(action));

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

    [Obsolete("Icon per window is now deprecated in GTK4, especially with Wayland", true)]
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIconName(this WindowHandle window, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_default_size", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetSize(this WindowHandle window, out int width, out int height);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_get_position", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetPosition(this WindowHandle window, out int x, out int y);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_resizable", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetResizable(this WindowHandle window, bool set);

    [DllImport(Libs.LibGtk, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddAction(this WindowHandle window, ActionHandle action);

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(this WindowHandle window, nint action);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_window_set_decorated", CallingConvention = CallingConvention.Cdecl)]
    extern static void _SetDecorated(this WindowHandle window, bool set);
    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewStatefulAction(string action, string? p, nint state);
}