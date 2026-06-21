using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

/// <summary>
/// A bar with contextual information. Banners are hidden by default.
/// </summary>
public class AdwBanner : Widget
{
    /// <summary>
    /// The title that is shown in the banner. The title will be shown centered or left-aligned depending on available space.
    /// </summary>
    public string? Title
    {
        get => GetTitle(this).PtrToString(false);
        set => SetTitle(this, value ?? "");
    }

    /// <summary>
    /// The label of the button
    /// </summary>
    public string? ButtonLabel
    {
        get => GetButtonLabel(this).PtrToString(false);
        set => SetButtonLabel(this, value ?? "");
    }

    /// <summary>
    /// Banners are hidden by default. With the helpof this property you can reveal or unreveal it.
    /// </summary>
    public bool IsRevealed
    {
        get => GetRevealed(this);
        set => SetRevealed(this, value);
    }

    /// <summary>
    /// Creates a new banner
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public static AdwBanner New(string title)
    {
        var banner = _New(title);
        banner.CheckDiagnostics();
        return banner;
    }

    /// <summary>
    /// When the banner button is clicked, this callback is called.
    /// </summary>
    /// <param name="click"></param>
    /// <returns></returns>
    public AdwBanner OnButtonClicked(Action click)
    {
        SignalConnect<TwoPointerDelegate>("button-clicked", (_, __) => click());
        return this;
    }

    /// <summary>
    /// Creates a banner with the help of a builder. This Ctor is automatically called when using a banner which is included in a template.ui.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    public AdwBanner(Builder builder, string? name = null) : base(builder, name) { }

    public AdwBanner() : base() { }

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


