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

    public Application WithDiagnostics()
    {
        Gtk.Diagnostics = true;
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
    public void AddActions(params GtkAction[] actions)
    {
        foreach (var action in actions)
        {
            if (action.Action != null)
            {
                var simpleAction = NewAction(action.Name, null);
                actionDelegateIds.Add(GtkDelegates.Add(action.Action));
                Gtk.SignalConnectAction(simpleAction, "activate", Marshal.GetFunctionPointerForDelegate(action.Action as Delegate), IntPtr.Zero, 0);
                AddAction(this, simpleAction);
            }
            else
            {
                // action.DelegateId = GtkDelegates.Add(action.StateChanged);
                // var state = action.StateParameterType == "s"
                //     ? NewString(action.State as string ?? "")
                //     : NewBool((bool?)action.State == true ? -1 : 0);
                // var simpleAction = NewStatefulAction(action.Name, action.StateParameterType, state);
                // action.action = simpleAction;
                // action.SignalId = Gtk.SignalConnectAction(simpleAction, "change-state", Marshal.GetFunctionPointerForDelegate(action.StateChanged), IntPtr.Zero, 0);
                // AddAction(GetInternalHandle(), simpleAction);
                // IActionMap.actions.Add(action.Name, simpleAction);
            }
        }

        AddWeakRef(() =>
        {
            foreach (var id in actionDelegateIds)
                GtkDelegates.Remove(id);
        });

        var accelEntries =
            actions
            .Where(n => n.Accelerator != null)
            .Select(n => new { Name = $"app.{n.Name}", n.Accelerator });
        foreach (var accelEntry in accelEntries)
            SetAccelsForAction(this, accelEntry.Name, [accelEntry.Accelerator, null]);
    }

    public void SetAccelsForAction(string action, [In] string?[] accels) => SetAccelsForAction(this, action, accels);

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

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewAction(string action, string? p);

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(Application application, ActionHandle action);

    readonly List<long> actionDelegateIds = [];
}

public static class ApplicationExtensions
{
    public static THandle Actions<THandle>(this THandle app, params GtkAction[] actions)
        where THandle : Application
        => app.SideEffect(app => app.AddActions(actions));
}

public record WindowBuilder(string Window, Builder Builder, Application Application);