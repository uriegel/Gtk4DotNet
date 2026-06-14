using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

class GtkActions
{
    public void AddActions(GObject actionMap, Application? app,  string groupName, params GtkAction[] actions)
    {
        this.actionMap = actionMap;
        foreach (var action in actions)
        {
            actionNames.Add(action.Name);
            if (action is SimpleAction simpleAction)
            {
                using var gAction = NewAction(simpleAction.Name, null);
                gAction.CheckDiagnostics();
                AddAction(actionMap, gAction);
                delegateKeys.Add(GtkDelegates.Instance.Add(simpleAction.Action, $"Action: {simpleAction.Name}"));
                SignalConnectAction(gAction, "activate", Marshal.GetFunctionPointerForDelegate(simpleAction.Action as Delegate), 0, 0);
            }
            else if (action is BoolAction boolAction)
            {
                var state = Variant.New(boolAction.InitialState);
                using var gAction = NewStatefulAction(action.Name, null, state);
                gAction.CheckDiagnostics();
                AddAction(actionMap, gAction);
                StateChangedDelegate boolStateChanged = (a, s) =>
                {
                    var state = HandleBoolState(a, s);
                    boolAction.StateChanged(state);
                };
                delegateKeys.Add(GtkDelegates.Instance.Add(boolStateChanged, $"Action: {action.Name}"));
                SignalConnectAction(gAction, "change-state", Marshal.GetFunctionPointerForDelegate(boolStateChanged), 0, 0);
            }
            else if (action is StringAction stringAction)
            {
                var state = Variant.New(stringAction.InitialState);
                using var gAction = NewStatefulAction(action.Name, "s", state);
                gAction.CheckDiagnostics();
                AddAction(actionMap, gAction);
                StateChangedDelegate? stringStateChanged = (a, s) =>
                {
                    var state = HandleStringState(a, s);
                    stringAction.StateChanged(state);
                };
                delegateKeys.Add(GtkDelegates.Instance.Add(stringStateChanged, $"Action: {action.Name}"));
                SignalConnectAction(gAction, "change-state", Marshal.GetFunctionPointerForDelegate(stringStateChanged), 0, 0);        
            }
        }

        actionMap.AddWeakRef(Cleanup);

        if (app != null)
        {
            var accelEntries =
                actions
                .Where(n => n.Accelerator != null)
                .Select(n => new { Name = $"{groupName}.{n.Name}", n.Accelerator });

            foreach (var accelEntry in accelEntries)
                app?.SetAccelsForAction(accelEntry.Name, [accelEntry.Accelerator, null]);
        }
    }

    public void Cleanup()
    {
        foreach (var name in actionNames)
            RemoveAction(actionMap, name);
        foreach (var id in delegateKeys)
            GtkDelegates.Instance.Remove(id);
    }

    static bool HandleBoolState(nint action, nint state)
    {
        ActionSetState(action, state);
        return Variant.GetBool(state);
    }

    string HandleStringState(nint action, nint state)
    {
        ActionSetState(action, state);
        return Variant.GetString(state);
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(GObject actionMap, ActionHandle action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_action_map_remove_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveAction(GObject actionMap, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewStatefulAction(string action, string? p, Variant state);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    extern static long SignalConnectAction(ActionHandle action, string name, nint callback, nint obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_set_state", CallingConvention = CallingConvention.Cdecl)]
    extern static void ActionSetState(nint action, nint state);

    delegate void StateChangedDelegate(nint action, nint state);

    GObject actionMap = null!;
    readonly List<string> actionNames = [];
    readonly List<long> delegateKeys = [];
}