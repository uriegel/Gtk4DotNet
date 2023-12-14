using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class HeaderBar
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_header_bar_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static HeaderBarHandle New();

    public static HeaderBarHandle TitleWidget(this HeaderBarHandle bar, WidgetHandle widget)
        => bar.SideEffect(h => h.SetTitleWidget(widget));

    public static HeaderBarHandle PackStart(this HeaderBarHandle bar, WidgetHandle widget)
        => bar.SideEffect(h => h._PackStart(widget));

    public static HeaderBarHandle PackEnd(this HeaderBarHandle bar, WidgetHandle widget)
        => bar.SideEffect(h => h._PackEnd(widget));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_header_bar_set_title_widget", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitleWidget(this HeaderBarHandle bar, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_header_bar_pack_start", CallingConvention = CallingConvention.Cdecl)]
    extern static void _PackStart(this HeaderBarHandle bar, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_header_bar_pack_end", CallingConvention = CallingConvention.Cdecl)]
    extern static void _PackEnd(this HeaderBarHandle bar, WidgetHandle widget);
}