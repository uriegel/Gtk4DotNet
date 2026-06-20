using System.Runtime.InteropServices;

namespace Gtk4DotNet;

// TODO Release ready

public class Image : Widget
{
    public static Image NewFromFile(string fileName)
    {
        var img = _NewFromFile(fileName);
        img.CheckDiagnostics();
        return img;
    }
    public static Image NewFromIconName(string iconName, IconSize size)
    {
        var img = _NewFromIconName(iconName, size);
        img.CheckDiagnostics();
        return img;
    }

    public static Image NewFromIcon(GIcon icon)
    {
        var img = _NewFromGIcon(icon);
        img.CheckDiagnostics();
        return img;
    }

    public void SetIcon(GIcon icon) => SetIcon(this, icon);

    public Image() : base() { }

    public Image(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    extern static Image _NewFromFile(string fileName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_gicon", CallingConvention = CallingConvention.Cdecl)]
    extern static Image _NewFromGIcon(GIcon icon);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_icon_name", CallingConvention = CallingConvention.Cdecl)]
    extern static Image _NewFromIconName(string iconName, IconSize size);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_set_from_gicon", CallingConvention = CallingConvention.Cdecl)]
    extern static Image SetIcon(Image image, GIcon icon);
}