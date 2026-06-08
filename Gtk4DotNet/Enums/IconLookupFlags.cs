namespace Gtk4DotNet;

public enum IconLookupFlags
{
    /// <summary>
    /// Perform a regular lookup.
    /// </summary>
    None,
    /// <summary>
    /// Try to always load regular icons, even when symbolic icon names are given.
    /// </summary>
    ForceRegular,
    /// <summary>
    /// Try to always load symbolic icons, even when regular icon names are given.
    /// </summary>
    ForceSymbolic,
    /// <summary>
    /// Starts loading the texture in the background so it is ready when later needed.
    /// </summary>
    Preload
}