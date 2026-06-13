using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class ApplicationWindow : Window
{
    public ApplicationWindow() : base() { }

    public ApplicationWindow(WindowBuilder builder) : base(builder.Builder, builder.Window)
        => SetApplication(this, builder.Application);

    /// <summary>
    /// Adds actions to this ActionMap.
    /// </summary>
    /// <remarks>
    /// Important: when setting actions with shortcuts, add those with more specific shortcuts like <c>&lt;Ctrl&gt;F3</c>  b e f o r e  those with less specific shortcuts like <c>F3</c>. 
    /// </remarks>
    /// <param name="actions"></param>
    public void AddActions(params GtkAction[] actions)
    {
        foreach (var action in actions)
            AddAction(this, action);

        AddWeakRef(() =>
        {
            foreach (var id in actionDelegateIds)
                GtkDelegates.Remove(id);
        });

        var accelEntries =
            actions
            .Where(n => n.Accelerator != null)
            .Select(n => new { Name = $"win.{n.Name}", n.Accelerator });

        var winApp = GetApplication();
        foreach (var accelEntry in accelEntries)
            winApp?.SetAccelsForAction(accelEntry.Name, [accelEntry.Accelerator, null]);
    }

    /// <summary>
    /// Adds actions to this ActionMap.
    /// </summary>
    /// <remarks>
    /// Important: when setting actions with shortcuts, add those with more specific shortcuts like <c>&lt;Ctrl&gt;F3</c>  b e f o r e  those with less specific shortcuts like <c>F3</c>. 
    /// </remarks>
    /// <param name="actions"></param>
    public void AddActionsEliminate(params GtkAction1[] actions)
    {
        foreach (var action in actions)
        {
            if (action.Action != null)
            {
                var simpleAction = NewAction(action.Name, null);
                actionDelegateIds.Add(GtkDelegates.Add(action.Action));
                Gtk.SignalConnectAction(simpleAction, "activate", Marshal.GetFunctionPointerForDelegate(action.Action as Delegate), 0, 0);
                AddAction(this, simpleAction);
            }
            else
            {
                actionDelegateIds.Add(GtkDelegates.Add(action.StateChanged));
                var state = action.StateParameterType == "s"
                    ? NewString(action.State as string ?? "")
                    : NewBool((bool?)action.State == true ? -1 : 0);
                var simpleAction = NewStatefulAction(action.Name, action.StateParameterType, state);
                Gtk.SignalConnectAction(simpleAction, "change-state", Marshal.GetFunctionPointerForDelegate(action.StateChanged), 0, 0);
                AddAction(this, simpleAction);
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
            .Select(n => new { Name = $"win.{n.Name}", n.Accelerator });

        var winApp = GetApplication();
        foreach (var accelEntry in accelEntries)
            winApp?.SetAccelsForAction(accelEntry.Name, [accelEntry.Accelerator, null]);
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static ActionHandle NewStatefulAction(string action, string? p, nint state);

    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(ApplicationWindow window, GtkAction action);

    // TODO eliminate
    [DllImport(Libs.LibGio, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddAction(ApplicationWindow window, ActionHandle action);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint NewString(string value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint NewBool(int value);

    readonly List<long> actionDelegateIds = [];
}

public static class ApplicationWindowExtensions
{
    public static THandle Actions<THandle>(this THandle win, params GtkAction[] actions)
        where THandle : ApplicationWindow
        => win.SideEffect(win => win.AddActions(actions));
}

// TODO
    // void FreeActions()
    // {
    //     foreach (var action in GetActionList())
    //     {
    //         GtkDelegates.Remove(action.DelegateId);
    //         Gtk.SignalDisconnect(action.action, action.SignalId);
    //         RemoveAction(GetInternalHandle(), action.Name);
    //         actions.Remove(action.Name);
    //         GObject.Unref(action.action);
    //     }
    // }
