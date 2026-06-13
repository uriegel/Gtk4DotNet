using System.Runtime.InteropServices;
using CsTools.Extensions;

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
    public void AddActions(params GtkAction[] actions) => this.actions.AddActions(this, GetApplication(), "win", actions);

    GtkActions actions = new();
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
