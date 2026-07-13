using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public interface IPaintable
{
    public double IntrinsicAspectRatio { get => GetIntrinsicAspectRatio(GetRaw()); }
    public double IntrinsicWidth { get => GetIntrinsicWidth(GetRaw()); }
    public double IntrinsicHeight { get => GetIntrinsicHeight(GetRaw()); }
    
    internal nint GetRaw();

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_paintable_get_intrinsic_aspect_ratio", CallingConvention = CallingConvention.Cdecl)]
    extern static double GetIntrinsicAspectRatio(nint paintable);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_paintable_get_intrinsic_width", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetIntrinsicWidth(nint paintable);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_paintable_get_intrinsic_height", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetIntrinsicHeight(nint paintable);
}