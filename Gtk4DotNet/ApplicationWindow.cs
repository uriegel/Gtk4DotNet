using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ApplicationWindow
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ApplicationWindowHandle New(ApplicationHandle application);

    // TODO use THandle everywhere
    // TODO Transfer this to WebWindowNetCore
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static ApplicationWindowHandle AddActions(this ApplicationWindowHandle win, IEnumerable<GtkAction> actions)
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

    public static ApplicationWindowHandle AddAction(this ApplicationWindowHandle window, ActionHandle action)
        => window.SideEffect(w => w._AddAction(action));

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(this WindowHandle window, nint action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddAction(this WindowHandle window, ActionHandle action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewStatefulAction(string action, string? p, nint state);
}
