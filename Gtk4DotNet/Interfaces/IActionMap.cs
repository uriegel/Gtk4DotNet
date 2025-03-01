using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public interface IActionMap
{
    public nint GetInternalHandle();
    IActionMap AddAction(ActionHandle action)
    {
        _AddAction(GetInternalHandle(), action);
        return this;
    }

    IActionMap AddActions(IEnumerable<GtkAction> actions)
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
                AddAction(GetInternalHandle(), simpleAction);
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
                AddAction(GetInternalHandle(), simpleAction);
            }
        }

        if (this is ApplicationHandle app)
            SetAccelsForAction(app, "app");
        else if (this is ApplicationWindowHandle win)
        {
            var winApp = win.GetApplication();
            if (!winApp.IsInvalid)
                SetAccelsForAction(winApp, "win");
            else
                Console.Error.WriteLine("Could not get application from window, so I could not attach the accelerators");
        }
        else if (this is WidgetHandle wh)
        {
            var root = wh.GetRoot();
            if (root is ApplicationWindowHandle aw)
            {
                Console.Error.WriteLine("Ja, habe die Applikation gefunden");
            }
            else
                Console.Error.WriteLine("Could not get window from widget, so I could not attach the accelerators");
        }
        return this;

        void SetAccelsForAction(ApplicationHandle app, string groupName)
        {
            var accelEntries =
                actions
                .Where(n => n.Accelerator != null)
                .Select(n => new { Name = $"{groupName}.{n.Name}", n.Accelerator });
            foreach (var accelEntry in accelEntries)
                Application.SetAccelsForAction(app, accelEntry.Name, [accelEntry.Accelerator, null]);
        }
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(nint window, nint action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddAction(nint window, ActionHandle action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewStatefulAction(string action, string? p, nint state);

    // [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_set_enabled", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void EnableAction(IntPtr action, int enabled);
}

public static class IActionMapExtensions
{
    public static THandle AddActions<THandle>(this THandle actionMap, params GtkAction[] actions)
        where THandle : IActionMap
    {
        actionMap.AddActions(actions);
        return actionMap;
    }
    
    public static THandle AddAction<THandle>(this THandle actionMap, ActionHandle action)
        where THandle : IActionMap
    {
        actionMap.AddAction(action);
        return actionMap;
    }
}

