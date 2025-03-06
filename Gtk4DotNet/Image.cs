using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Image
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    public extern static ImageHandle NewFromFile(string fileName);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_icon_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static ImageHandle NewFromIconName(string iconName, IconSize size);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_set_from_icon_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetFromIconName(this ImageHandle image, string iconName, IconSize size);
}