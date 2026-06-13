using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class SimpleActionGroup : FloatingObject
{
    public static SimpleActionGroup New()
    {
        var group = _New();
        group.CheckDiagnostics();
        return group;
    }

    public void AddActions(params GtkAction[] actions)
    {
        foreach (var action in actions)
        {
            if (action.Action != null)
            {
                var simpleAction = NewAction(action.Name, null);
                //action.action = simpleAction;
                action.DelegateId = GtkDelegates.Add(action.Action);
                action.SignalId = Gtk.SignalConnectAction(simpleAction, "activate", Marshal.GetFunctionPointerForDelegate(action.Action as Delegate), IntPtr.Zero, 0);
                AddAction(this, simpleAction);
            }
            // else
            // {
            //     action.DelegateId = GtkDelegates.Add(action.StateChanged);
            //     var state = action.StateParameterType == "s"
            //         ? NewString(action.State as string ?? "")
            //         : NewBool((bool?)action.State == true ? -1 : 0);
            //     var simpleAction = NewStatefulAction(action.Name, action.StateParameterType, state);
            //     action.action = simpleAction;
            //     action.SignalId = Gtk.SignalConnectAction(simpleAction, "change-state", Marshal.GetFunctionPointerForDelegate(action.StateChanged), IntPtr.Zero, 0);
            //     AddAction(GetInternalHandle(), simpleAction);
            //     IActionMap.actions.Add(action.Name, simpleAction);
            // }
        }
    }


    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_group_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SimpleActionGroup _New();

    [DllImport(Libs.LibGtk, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(SimpleActionGroup group, ActionHandle action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewAction(string action, string? p);
}
