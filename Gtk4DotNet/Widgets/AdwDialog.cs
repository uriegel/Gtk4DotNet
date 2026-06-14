using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class AdwDialog : Widget
{
    public static void PresentFromTemplate(string template, string name, Widget parent, Func<Builder, string, AdwDialog>? ctor = null)
    {
        using var builder = Builder.FromDotNetResource(template);
        var dialog = ctor?.Invoke(builder, name) ?? new AdwDialog(builder, name);
        dialog.Present(parent);
    }

    public void Present(Widget parent) => Present(this, parent);

    public void SetDefaultWidget(Widget widget) => SetDefaultWidget(this, widget);

    public void CloseDialog() => Close(this);

    public AdwDialog() : base() {}

    protected AdwDialog(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_present", CallingConvention = CallingConvention.Cdecl)]
    extern static void Present(AdwDialog dialog, Widget parent);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_close", CallingConvention = CallingConvention.Cdecl)]
    extern static void Close(AdwDialog dialog);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_dialog_set_default_widget", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDefaultWidget(AdwDialog dialog, Widget widget);
    
}
