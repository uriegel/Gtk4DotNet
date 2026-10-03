using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Image : Widget
{
    /// <summary>
    /// Creates an empty image
    /// </summary>
    /// <returns></returns>
    public Image()
    {
        SetInternalHandle(New());
        CheckDiagnostics();
    }

    /// <summary>
    /// Creates an image from a file
    /// </summary>
    /// <param name="fileName">The path of the image file</param>
    /// <returns></returns>
    public Image(string fileName)
    {
        SetInternalHandle(NewFromFile(fileName));
        CheckDiagnostics();
    }
    
    /// <summary>
    /// Creates an image with the help of the icon name
    /// </summary>
    /// <param name="iconName">Icon name of the image</param>
    /// <param name="size">Desired size of the image</param>
    /// <returns></returns>
    public Image(string iconName, IconSize size)
    {
        SetInternalHandle(NewFromIconName(iconName, size));
        CheckDiagnostics();
    }

    /// <summary>
    /// Creates an image from a <see cref="GIcon"/>
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    public Image(GIcon icon)
    {
        SetInternalHandle(NewFromGIcon(icon));
        CheckDiagnostics();
    }

    /// <summary>
    /// Sets a <see cref="GIcon"/> to this image.
    /// </summary>
    /// <param name="icon"></param>
    public void SetIcon(GIcon icon) => SetIcon(this, icon);

    public void SetFromIconName(string icon) => SetFromIconName(this, icon);

    public Image(Builder builder, string? name = null) : base(builder, name) { }

    public Image(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewFromFile(string fileName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_gicon", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewFromGIcon(GIcon icon);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewFromIconName(string iconName, IconSize size);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_set_from_gicon", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetIcon(Image image, GIcon icon);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_set_from_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetFromIconName(Image image, string icon);
}