namespace Gtk4DotNet;

/// <summary>
/// An Adwaita ApplicationWindow instead of a Gtk <see cref="ApplicationWindow"/>. This window integrates best with modern Gnome and has initially no title bar.
/// </summary>
/// <remarks>
/// It is recommended to build a window from a .NET resource template.ui.
/// </remarks>
public class AdwApplicationWindow : ApplicationWindow
{
    public AdwApplicationWindow() : base() { }

    public AdwApplicationWindow(WindowBuilder builder) : base(builder) { }
}
