using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class HeaderBar
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_header_bar_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static HeaderBarHandle New();

    public static HeaderBarHandle TitleWidget(this HeaderBarHandle bar, WidgetHandle widget)
        => bar.SideEffect(h => h.SetTitleWidget(widget));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_header_bar_set_title_widget", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitleWidget(this HeaderBarHandle bar, WidgetHandle widget);
}