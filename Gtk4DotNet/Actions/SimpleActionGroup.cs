using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class SimpleActionGroup : GObject
{
    public string GroupName { get; private set; } = null!;
    public static SimpleActionGroup New(string groupName)
    {
        var group = _New();
        group.GroupName = groupName;
        group.CheckDiagnostics();
        return group;
    }

    public void AddActions(params GtkAction[] actions) => this.actions.AddActions(this, null, GroupName, actions);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_group_new", CallingConvention = CallingConvention.Cdecl)]
    extern static SimpleActionGroup _New();

    readonly GtkActions actions = new(false);
}
