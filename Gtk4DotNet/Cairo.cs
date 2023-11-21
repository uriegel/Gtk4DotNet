using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Cairo
{
    [DllImport(Libs.LibGtk, EntryPoint = "cairo_create", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Create(IntPtr surface);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_paint", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Paint(this IntPtr cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSourceSurface(this IntPtr cairo,  IntPtr surface, double x, double y);

    // TODO Deprecated
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_surface_create_similar_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static SurfaceHandle SurfaceCreateSimilar(this NativeHandle native, CairoContent cairoContent, int width, int height);
    
    [DllImport(Libs.LibGtk, EntryPoint = "cairo_surface_destroy", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SurfaceDestroy(this IntPtr surface);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_antialias", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetAntiAlias(this IntPtr context, CairoAntialias antialias);


    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_join", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetLineJoin(this IntPtr context, LineJoin lineJoin);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_cap", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetLineCap(this IntPtr context, LineCap lineCap);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_translate", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Translate(this IntPtr context, double x, double y);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_stroke_preserve", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void StrokePreserve(this IntPtr context);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_arc_negative", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void ArcNegative(this IntPtr context, double x, double y, double radius, double angle1, double angle2);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_line_to", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void LineTo(this IntPtr context, double x, double y);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_rectangle", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Rectangle(this IntPtr context, double x, double y, double width, double height);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_rgb", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetSourceRgb(this IntPtr context, double r, double g, double b);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_fill", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void CairoFill(this IntPtr context);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_move_to", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void CairoMoveTo(this IntPtr context, double x, double y);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_arc", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Arc(this IntPtr context, double x, double y, double radius, double angle1, double angle2);

    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_surface", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void SetSourceSurface(this IntPtr cairo,  IntPtr surface, double x, double y);
    
    // [DllImport(Libs.LibGtk, EntryPoint = "cairo_destroy", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void CairoDestroy(this IntPtr cairo);
}

