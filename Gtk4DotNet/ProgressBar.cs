using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class ProgressBar
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ProgressBarHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_progress_bar_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    public static ProgressBarHandle ShowText(this ProgressBarHandle progressBar, bool show = true)
        => progressBar.SideEffect(p => p.SetShowText(show));

    public static ProgressBarHandle Fraction(this ProgressBarHandle progressBar, double fraction)
        => progressBar.SideEffect(p => p.SetFraction(fraction));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_set_show_text", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetShowText(this ProgressBarHandle progressBar, bool show);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_progress_bar_set_fraction", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFraction(this ProgressBarHandle progressBar, double fraction);
}
