using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Image : Widget
{
    /// <summary>
    /// Creates an empty image
    /// </summary>
    /// <returns></returns>
    public static Image New()
    {
        var img = _New();
        img.CheckDiagnostics();
        return img;
    }

    /// <summary>
    /// Creates an image from a file
    /// </summary>
    /// <param name="fileName">The path of the image file</param>
    /// <returns></returns>
    public static Image NewFromFile(string fileName)
    {
        var img = _NewFromFile(fileName);
        img.CheckDiagnostics();
        return img;
    }
    
    /// <summary>
    /// Creates an image with the help of the icon name
    /// </summary>
    /// <param name="iconName">Icon name of the image</param>
    /// <param name="size">Desired size of the image</param>
    /// <returns></returns>
    public static Image NewFromIconName(string iconName, IconSize size)
    {
        var img = _NewFromIconName(iconName, size);
        img.CheckDiagnostics();
        return img;
    }

    /// <summary>
    /// Creates an image from a <see cref="GIcon"/>
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static Image NewFromIcon(GIcon icon)
    {
        var img = _NewFromGIcon(icon);
        img.CheckDiagnostics();
        return img;
    }

    /// <summary>
    /// Sets a <see cref="GIcon"/> to this image.
    /// </summary>
    /// <param name="icon"></param>
    public void SetIcon(GIcon icon) => SetIcon(this, icon);

    public void SetFromIconName(string icon) => SetFromIconName(this, icon);


    public Image() : base() { }

    public Image(Builder builder, string? name = null) : base(builder, name) { }

    public Image(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new", CallingConvention = CallingConvention.Cdecl)]
    extern static Image _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    extern static Image _NewFromFile(string fileName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_gicon", CallingConvention = CallingConvention.Cdecl)]
    extern static Image _NewFromGIcon(GIcon icon);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static Image _NewFromIconName(string iconName, IconSize size);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_set_from_gicon", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIcon(Image image, GIcon icon);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_set_from_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFromIconName(Image image, string icon);
}