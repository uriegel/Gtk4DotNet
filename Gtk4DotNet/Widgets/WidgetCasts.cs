namespace Gtk4DotNet;

public static class WidgetCasts
{
    public static Window AsWindow(this Widget widget) => new(widget.GetInternalHandle());
}