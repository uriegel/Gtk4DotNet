using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class IconPaintable : GObject
{
    public GFile GetFile()
    {
        var file = GetFile(this);
        file.CheckDiagnostics();
        return file;
    }
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_paintable_get_file", CallingConvention = CallingConvention.Cdecl)]
    extern static GFile GetFile(IconPaintable paintable);
}