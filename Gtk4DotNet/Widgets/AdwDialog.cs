using System.Runtime.InteropServices;

namespace Gtk4DotNet;

/// <summary>
/// An Adwaita dialog
/// </summary>
public class AdwDialog : Widget
{
    /// <summary>
    /// Shows an Adwaita dialog which is defined in a template.ui included as a .NET resource
    /// </summary>
    /// <param name="template">template.ui which defines this dialog included as a .NET resource</param>
    /// <param name="name">The name of this dialog in the template</param>
    /// <param name="parent">A window which is the parent of this dialog</param>
    /// <param name="ctor">Constructor to create an <see cref="AdwDialog"/> or a custom dialog inherited from <see cref="AdwDialog"/> with the help of a <see cref="Builder"/>.</param>
    public static void PresentFromTemplate(string template, string name, Widget parent, Func<Builder, string, AdwDialog>? ctor = null)
    {
        using var builder = Builder.FromDotNetResource(template);
        var dialog = ctor?.Invoke(builder, name) ?? new AdwDialog(builder, name);
        dialog.Present(parent);
    }

    /// <summary>
    /// Shows the Adwaita dialog
    /// </summary>
    /// <param name="parent"></param>
    public void Present(Widget parent) => Present(this, parent);

    /// <summary>
    /// Sets a widget shich should be the default widget
    /// </summary>
    /// <param name="widget"></param>
    public void SetDefaultWidget(Widget widget) => SetDefaultWidget(this, widget);

    /// <summary>
    /// Closes this Adwaita dialog
    /// </summary>
    public void CloseDialog() => Close(this);

    public AdwDialog() : base() { }

    public AdwDialog(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_present", CallingConvention = CallingConvention.Cdecl)]
    extern static void Present(AdwDialog dialog, Widget parent);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_close", CallingConvention = CallingConvention.Cdecl)]
    extern static void Close(AdwDialog dialog);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_set_default_widget", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDefaultWidget(AdwDialog dialog, Widget widget);

}
