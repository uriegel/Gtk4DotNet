using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Image
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_image_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    public extern static ImageHandle NewFromFile(string fileName);
}