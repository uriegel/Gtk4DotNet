using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class Surface : GObject
{
    protected override bool ReleaseHandle()
        => true.SideEffect(_ => SurfaceDestroy(handle));
        
    [DllImport(Libs.LibGtk, EntryPoint = "cairo_surface_destroy", CallingConvention = CallingConvention.Cdecl)]
    extern static void SurfaceDestroy(nint surface);
}
