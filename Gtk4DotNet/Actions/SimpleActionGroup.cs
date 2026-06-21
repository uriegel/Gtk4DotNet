using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// A simple GTK Action group that can be assigned to a widget
/// </summary>
public class SimpleActionGroup : GObject
{
    /// <summary>
    /// Name of the action group
    /// </summary>
    public string GroupName { get; private set; } = null!;
    
    /// <summary>
    /// Creates a simple action group
    /// </summary>
    /// <param name="groupName">Name of the action group</param>
    /// <returns></returns>
    public static SimpleActionGroup New(string groupName)
    {
        var group = _New();
        group.GroupName = groupName;
        group.CheckDiagnostics();
        return group;
    }

    /// <summary>
    /// Append all actions to the action group
    /// </summary>
    /// <param name="actions"></param>
    public void AddActions(params GtkAction[] actions) => this.actions.AddActions(this, null, GroupName, actions);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_group_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SimpleActionGroup _New();

    readonly GtkActions actions = new(false);
}
