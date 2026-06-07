using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Application : GObject //, IActionMap
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

    public Application WithDiagnostics()
    {
        Gtk.Diagnostics = true;
        CheckDiagnostics();
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

    [DllImport(Libs.LibAdw, EntryPoint = "adw_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Application _NewAdw(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Application _New(string id, int flags = 0);

    [DllImport(Libs.LibGtk, EntryPoint = "g_application_run", CallingConvention = CallingConvention.Cdecl)]
    extern static int _Run(Application app, int c, nint a);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ApplicationWindow NewWindow(Application app);
}

public record WindowBuilder(string Window, Builder Builder, Application Application);