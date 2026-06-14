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
            var state = action.BoolAction != null
                ? NewBool(action.InitialBoolState ? -1 : 0)
                : action.StringAction != null
                ? NewString(action.InitialStringState ?? "")
                : 0;
            /* TODO using*/ var gAction = action.Action != null
                                ? NewAction(action.Name, null)
                                : NewStatefulAction(action.Name, action.StringAction != null ? "s" : null, state);

            // TODO free state
            gAction.CheckDiagnostics();

            AddAction(actionMap, gAction);
            actionNames.Add(action.Name);

            StateChangedDelegate? boolStateChanged = action.BoolAction != null
                ? (a, s) =>
                {
                    var state = HandleBoolState(a, s);
                    action.BoolAction(state);
                }
                : null;

            StateChangedDelegate? stringStateChanged = action.StringAction != null
                ? (a, s) =>
                {
                    var state = HandleStringState(a, s);
                    action.StringAction(state);
                }
                : null;

            var id = action.Action != null
                ? GtkDelegates.Instance.Add(action.Action, $"Action: {action.Name}")
                : boolStateChanged != null
                ? GtkDelegates.Instance.Add(boolStateChanged, $"Action: {action.Name}")
                : stringStateChanged != null
                ? GtkDelegates.Instance.Add(stringStateChanged, $"Action: {action.Name}")
                : 0;
            delegateKeys.Add(id);
            if (action.Action != null)
                SignalConnectAction(gAction, "activate", Marshal.GetFunctionPointerForDelegate(action.Action as Delegate), 0, 0);
            else if (boolStateChanged != null)
                SignalConnectAction(gAction, "change-state", Marshal.GetFunctionPointerForDelegate(boolStateChanged), 0, 0);
            else if (stringStateChanged != null)
                SignalConnectAction(gAction, "change-state", Marshal.GetFunctionPointerForDelegate(stringStateChanged), 0, 0);        
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

    bool HandleBoolState(nint action, nint state)
    {
        ActionSetState(action, state);
        return GetBool(state) != 0;
        // TODO check free
    }

    string HandleStringState(nint action, nint state)
    {
        ActionSetState(action, state);
        var strptr = GetString(state, IntPtr.Zero);
        return Marshal.PtrToStringAuto(strptr) ?? "";
        // TODO check free
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(GObject actionMap, ActionHandle action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_action_map_remove_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveAction(GObject actionMap, string name);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewStatefulAction(string action, string? p, nint state);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    extern static long SignalConnectAction(ActionHandle action, string name, nint callback, nint obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewBool(int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint NewString(string value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_set_state", CallingConvention = CallingConvention.Cdecl)]
    extern static void ActionSetState(nint action, nint state);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetBool(nint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetString(nint value, nint size);

    delegate void StateChangedDelegate(nint action, nint state);

    GObject actionMap = null!;
    readonly List<string> actionNames = [];
    readonly List<long> delegateKeys = [];
}