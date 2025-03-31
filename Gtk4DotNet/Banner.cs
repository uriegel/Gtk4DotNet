using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Banner
{
    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static BannerHandle New(string title);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_set_title", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetTitle(this BannerHandle banner, string title);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_set_button_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetButtonLabel(this BannerHandle banner, string label);

    public static BannerHandle ButtonLabel(this BannerHandle banner, string label)
        => banner.SideEffect(b => b.SetButtonLabel(label));

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_set_revealed", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetRevealed(this BannerHandle banner, bool revealed);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_get_revealed", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool GetRevealed(this BannerHandle banner);

    public static BannerHandle OnButtonClicked(this BannerHandle banner, Action click)
        => banner.SideEffect(b => Gtk.SignalConnect<TwoPointerDelegate>(b, "button-clicked", (_, __) => click()));
}
