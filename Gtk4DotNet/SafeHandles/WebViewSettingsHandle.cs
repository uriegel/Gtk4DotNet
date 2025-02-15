namespace GtkDotNet.SafeHandles;

public class WebViewSettingsHandle : ObjectFloatingHandle
{
    public WebViewSettingsHandle() : base() { }

    public bool AllowModalDialogs
    {
        get => this.GetBool("allow-modal-dialogs");
        set => this.SetBool("allow-modal-dialogs", value);
    }
    public bool AutoLoadImages
    {
        get => this.GetBool("auto-load-images");
        set => this.SetBool("auto-load-images", value);
    }
    public string? CursiveFontFamily
    {
        get => this.GetString("cursive-font-family");
        set => this.SetString("cursive-font-family", value);
    }
    // public string	default-charset	
    // public string	default-font-family	
    // public int	default-font-size	
    // public int	default-monospace-font-size	
    // public bool	draw-compositing-indicators	
    // public bool	enable-accelerated-2d-canvas	
    // public bool	enable-caret-browsing	
    public bool EnableDeveloperExtras
    {
        get => this.GetBool("enable-developer-extras");
        set => this.SetBool("enable-developer-extras", value);
    }
    // public bool	enable-dns-prefetching	
    // public bool	enable-frame-flattening	
    // public bool	enable-fullscreen	
    // public bool	enable-html5-database	
    // public bool	enable-html5-local-storage	
    // public bool	enable-hyperlink-auditing	
    // public bool	enable-java	
    // public bool	enable-javascript	
    // public bool	enable-media-stream	
    // public bool	enable-mediasource	
    // public bool	enable-offline-web-application-cache	
    // public bool	enable-page-cache	
    // public bool	enable-plugins	
    // public bool	enable-private-browsing	
    // public bool	enable-resizable-text-areas	
    // public bool	enable-site-specific-quirks	
    // public bool	enable-smooth-scrolling	
    // public bool	enable-spatial-navigation	
    // public bool	enable-tabs-to-links	
    // public bool	enable-webaudio	
    // public bool	enable-webgl	
    // public bool	enable-write-console-messages-to-stdout	
    // public bool	enable-xss-auditor	
    // public string	fantasy-font-family	
    // public bool	javascript-can-access-clipboard	
    // public bool	javascript-can-open-windows-automatically	
    // public bool	load-icons-ignoring-image-load-setting	
    // public bool	media-playback-allows-inline	
    // public bool	media-playback-requires-user-gesture	
    // public int	minimum-font-size	
    // public string	monospace-font-family	
    // public string	pictograph-font-family	
    // public bool	print-backgrounds	
    // public string	sans-serif-font-family	
    // public string	serif-font-family	
    // public string	user-agent	
    // public bool	zoom-text-only	
}

