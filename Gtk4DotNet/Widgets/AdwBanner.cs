using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// TODO Release ready

public class AdwBanner : Widget
{
    public string? Title
    {
        get => GetTitle(this).PtrToString(false);
        set => SetTitle(this, value ?? "");
    }

    public string? ButtonLabel
    {
        get => GetButtonLabel(this).PtrToString(false);
        set => SetButtonLabel(this, value ?? "");
    }

    public bool IsRevealed
    {
        get => GetRevealed(this);
        set => SetRevealed(this, value);
    }

    public static AdwBanner New(string title)
    {
        var banner = _New(title);
        banner.CheckDiagnostics();
        return banner;
    }

    public AdwBanner OnButtonClicked(Action click)
    {
        SignalConnect<TwoPointerDelegate>("button-clicked", (_, __) => click());
        return this;
    }

    public AdwBanner() : base() { }

    public AdwBanner(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_new", CallingConvention = CallingConvention.Cdecl)]
    extern static AdwBanner _New(string title);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_get_title", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetTitle(AdwBanner banner);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_set_title", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitle(AdwBanner banner, string title);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_get_button_label", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetButtonLabel(AdwBanner banner);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_set_button_label", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetButtonLabel(AdwBanner banner, string label);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_set_revealed", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetRevealed(AdwBanner banner, bool revealed);

    [DllImport(Libs.LibAdw, EntryPoint = "adw_banner_get_revealed", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetRevealed(AdwBanner banner);
}


