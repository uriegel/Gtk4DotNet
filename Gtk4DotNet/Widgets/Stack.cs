using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// The GtkStack widget is a container which only shows one of its children at a time. 
/// In contrast to GtkNotebook, GtkStack does not provide a means for users to change the visible child. 
/// Instead, the GtkStackSwitcher widget can be used with GtkStack to provide this functionality.
/// Transitions between pages can be animated as slides or fades. This can be controlled with gtk_stack_set_transition_type(). 
/// These animations respect the GtkSettings:gtk-enable-animations setting.
/// </summary>
public class Stack : Widget
{
    /// <summary>
    /// Adds a child to stack. The child is identified by the name. The title will be used by GtkStackSwitcher to represent child in a tab bar, so it should be short.
    /// </summary>
    /// <param name="child"></param>
    /// <param name="name"></param>
    /// <param name="title"></param>
    public void AddTitled(Widget child, string name, string title) => AddTitled(this, child, name, title);
    
    public Stack() : base() { }

    public Stack(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_stack_add_titled", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddTitled(Stack stack, Widget child, string name, string title);
}
