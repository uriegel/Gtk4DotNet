using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public interface IActionMap
{
    internal nint GetInternalHandle();
    
    IActionMap AddAction(ActionHandle action)
    {
        _AddAction(GetInternalHandle(), action);
        return this;
    }

    public static nint GetAction(string name)
        => actions[name];

    void FreeActions()
    {
        foreach (var action in GetActionList())
        {
            GtkDelegates.Remove(action.DelegateId);
            Gtk.SignalDisconnect(action.action, action.SignalId);
            RemoveAction(GetInternalHandle(), action.Name);
            actions.Remove(action.Name);
            GObject.Unref(action.action);
        }
    }

    List<GtkAction> GetActionList();

    IActionMap AddActions(IEnumerable<GtkAction> actions)
    {
        var actionList = GetActionList();
        var gtkActions = actions.OfType<GtkAction>();
        foreach (var action in gtkActions)
        {
            actionList.Add(action);
            if (action.Action != null)
            {
                var simpleAction = NewAction(action.Name, null);
                action.action = simpleAction;
                action.DelegateId = GtkDelegates.Add(action.Action);
                action.SignalId = Gtk.SignalConnectAction(simpleAction, "activate", Marshal.GetFunctionPointerForDelegate(action.Action as Delegate), IntPtr.Zero, 0);
                AddAction(GetInternalHandle(), simpleAction);
                IActionMap.actions.Add(action.Name, simpleAction);
            }
            else
            {
                action.DelegateId = GtkDelegates.Add(action.StateChanged);
                var state = action.StateParameterType == "s"
                    ? NewString(action.State as string ?? "")
                    : NewBool((bool?)action.State == true ? -1 : 0);
                var simpleAction = NewStatefulAction(action.Name, action.StateParameterType, state);
                action.action = simpleAction;
                action.SignalId = Gtk.SignalConnectAction(simpleAction, "change-state", Marshal.GetFunctionPointerForDelegate(action.StateChanged), IntPtr.Zero, 0);
                AddAction(GetInternalHandle(), simpleAction);
                IActionMap.actions.Add(action.Name, simpleAction);
            }
        }

        if (this is Application app)
            SetAccelsForAction(app, "app");
        else if (this is ApplicationWindow win)
        {
            var winApp = win.GetApplication();
            if (!winApp.IsInvalid)
                SetAccelsForAction(winApp, "win");
            else
                Console.Error.WriteLine("Could not get application from window, so I could not attach the accelerators");
        }
        else if (this is Widget wh)
        {
            var root = wh.GetRoot();
            if (root is ApplicationWindow aw)
                Console.Error.WriteLine("Found window from widget");
            else
                Console.Error.WriteLine("Could not get window from widget, so I could not attach the accelerators");
        }
        return this;

        void SetAccelsForAction(Application app, string groupName)
        {
            var accelEntries =
                actions
                .Where(n => n.Accelerator != null)
                .Select(n => new { Name = $"{groupName}.{n.Name}", n.Accelerator });
            foreach (var accelEntry in accelEntries)
                app.SetAccelsForAction(accelEntry.Name, [accelEntry.Accelerator, null]);
        }
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(nint window, nint action);

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_remove_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveAction(nint window, string name);
    
    [DllImport(Libs.LibGtk, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AddAction(nint window, ActionHandle action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewStatefulAction(string action, string? p, nint state);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr NewString(string value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr NewBool(int value);

    // [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_set_enabled", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void EnableAction(IntPtr action, int enabled);

    internal static int GetActionsCount() => actions.Count;
    static readonly Dictionary<string, nint> actions = [];
}

public static class IActionMapExtensions
{
    /// <summary>
    /// Adds actions to this ActionMap.
    /// </summary>
    /// <remarks>
    /// Important: when setting actions with shortcuts, add those with more specific shortcuts like <c>&lt;Ctrl&gt;F3</c>  b e f o r e  those with less specific shortcuts like <c>F3</c>. 
    /// </remarks>
    /// <typeparam name="THandle"></typeparam>
    /// <param name="actionMap"></param>
    /// <param name="actions"></param>
    /// <returns></returns>
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
    
    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_set_enabled", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint SetEnabled(this nint action, bool enabled);
}

