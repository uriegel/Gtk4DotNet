using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Application : GObject
{
    public static Application New(string id, int flags = 0)
    {
        var app = _New(id, 0);
        Gtk.Init();
        return app;
    }

    public static Application NewAdwaita(string id, int flags = 0)
    {
        var app = _NewAdw(id, 0);
        Gtk.Init();
        return app;
    }

    public Application OnActivate(Action<Application> activate)
        => this.SideEffect(_ => SignalConnect<OnePointerDelegate>("activate", _ => activate(this)));

    public Application WithDiagnostics(bool gobjectTracing = false)
    {
        Gtk.Diagnostics = true;
        Gtk.GObjectTracing = gobjectTracing;
        CheckDiagnostics();
        return this;
    }

    public Application WithWebKit()
    {
        GType.Get(GTypeEnum.WebKitWebView);
        return this;
    }

    public int Run(int c = 0, nint a = 0)
    {
        var result = _Run(this, c, a);
        Dispose();
        if (Gtk.Diagnostics)
            Gtk.ShowDiagnostics();
        return result;
    }

    public ApplicationWindow NewWindow()
    {
        var res = NewWindow(this);
        res.CheckDiagnostics();
        return res;
    }

    public ApplicationWindow WindowFromBuilder(string template, string window, Func<WindowBuilder, ApplicationWindow> creator)
    {
        using var builder = Builder.FromDotNetResource(template);
        var res = creator(new(window, builder, this));
        res.CheckDiagnostics();
        return res;
    }

    /// <summary>
    /// Adds actions to this ActionMap.
    /// </summary>
    /// <remarks>
    /// Important: when setting actions with shortcuts, add those with more specific shortcuts like <c>&lt;Ctrl&gt;F3</c>  b e f o r e  those with less specific shortcuts like <c>F3</c>. 
    /// </remarks>
    /// <param name="actions"></param>
    /// <returns></returns>
    public void AddActions(params GtkAction[] actions) => this.actions.AddActions(this, this, "app", actions);

    internal void SetAccelsForAction(string action, [In] string?[] accels) => SetAccelsForAction(this, action, accels);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Application _NewAdw(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Application _New(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "g_application_run", CallingConvention = CallingConvention.Cdecl)]
    extern static int _Run(Application app, int c, nint a);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationWindow NewWindow(Application app);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_set_accels_for_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetAccelsForAction(Application app, string action, [In] string?[] accels);

    readonly GtkActions actions = new(true);
}

public static class ApplicationExtensions
{
    public static THandle Actions<THandle>(this THandle app, params GtkAction[] actions)
        where THandle : Application
        => app.SideEffect(app => app.AddActions(actions));
}

public record WindowBuilder(string Window, Builder Builder, Application Application);