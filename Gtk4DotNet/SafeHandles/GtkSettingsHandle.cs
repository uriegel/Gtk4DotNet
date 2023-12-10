namespace GtkDotNet.SafeHandles;

public class GtkSettingsHandle : ObjectFloatingHandle
{
    public GtkSettingsHandle() : base() {}

    // color-hash
    // alternative-button-order
    // alternative-sort-arrows
    // application-prefer-dark-theme
    // auto-mnemonics
    // button-images
    // can-change-accels
    // color-palette
    // color-scheme
    // cursor-aspect-ratio
    // cursor-blink
    // cursor-blink-time
    // cursor-blink-timeout
    // cursor-theme-name
    // cursor-theme-size
    // decoration-layout
    // dialogs-use-header
    // dnd-drag-threshold
    // double-click-distance
    // double-click-time
    // enable-accels
    // enable-animations
    // enable-event-sounds
    // enable-input-feedback-sounds
    // enable-mnemonics
    // enable-primary-paste
    // enable-tooltips
    // entry-password-hint-timeout
    // entry-select-on-focus
    // error-bell
    // fallback-icon-theme
    // file-chooser-backend
    // font-name
    // fontconfig-timestamp
    // icon-sizes
    // icon-theme-name
    // im-module
    // im-preedit-style
    // im-status-style
    // key-theme-name
    // keynav-cursor-only
    // keynav-use-caret
    // keynav-wrap-around
    // label-select-on-focus
    // long-press-time
    // menu-bar-accel
    // menu-bar-popup-delay
    // menu-images
    // menu-popdown-delay
    // menu-popup-delay
    // modules
    // overlay-scrolling
    // primary-button-warps-slider
    // print-backends
    // print-preview-command
    // recent-files-enabled
    // recent-files-limit
    // recent-files-max-age
    // scrolled-window-placement
    // shell-shows-app-menu
    // shell-shows-desktop
    // shell-shows-menubar
    // show-input-method-menu
    // show-unicode-menu
    // sound-theme-name
    // split-cursor
    public string? ThemeName { get => this.GetString("gtk-theme-name"); } 
    // timeout-expand
    // timeout-initial
    // timeout-repeat
    // titlebar-double-click
    // titlebar-middle-click
    // titlebar-right-click
    // toolbar-icon-size
    // toolbar-style
    // tooltip-browse-mode-timeout
    // tooltip-browse-timeout
    // tooltip-timeout
    // touchscreen-mode
    // visible-focus
    // xft-antialias

    // xft-dpi
    // xft-hinting
    // xft-hintstyle
    // xft-rgba

}
