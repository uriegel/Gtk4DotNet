using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

/// <summary>
/// A <see cref="Window"/> subclass that integrates with <see cref="Application"/>.
/// </summary>
/// <remarks>
/// It is recommended to build a window from a .NET resource template.ui.
/// </remarks>
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

    public void SetHelpOverlay(Window window) => SetHelpOverlay(this, window);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_application_window_set_help_overlay", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetHelpOverlay(ApplicationWindow app, Window window);

    readonly GtkActions actions = new(true);
}

public static class ApplicationWindowExtensions
{
    /// <summary>
    /// Adds actions to this ActionMap.
    /// </summary>
    /// <typeparam name="THandle"></typeparam>
    /// <param name="win"></param>
    /// <param name="actions"></param>
    /// <returns>The ApplicationWindow for chaining method calls</returns>
    public static THandle Actions<THandle>(this THandle win, params GtkAction[] actions)
        where THandle : ApplicationWindow
        => win.SideEffect(win => win.AddActions(actions));
}
