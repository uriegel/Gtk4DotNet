namespace Gtk4DotNet;

public enum ContentFit
{
    /// <summary>
    /// Make the content fill the entire allocation, without taking its aspect ratio in consideration. 
    /// The resulting content will appear as stretched if its aspect ratio is different from the allocation aspect ratio.
    /// </summary>
    Fill,
    /// <summary>
    /// Scale the content to fit the allocation, while taking its aspect ratio in consideration. 
    /// The resulting content will appear as letterboxed if its aspect ratio is different from the allocation aspect ratio.
    /// </summary>
    Contain,
    /// <summary>
    /// Cover the entire allocation, while taking the content aspect ratio in consideration. 
    /// The resulting content will appear as clipped if its aspect ratio is different from the allocation aspect ratio.
    /// </summary>
    Cover,
    /// <summary>
    /// The content is scaled down to fit the allocation, if needed, otherwise its original size is used.
    /// </summary>
    ScaleDown
}