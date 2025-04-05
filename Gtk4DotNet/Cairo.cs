using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Cairo
{
    [DllImport(Libs.LibGtk, EntryPoint = "cairo_create", CallingConvention = CallingConvention.Cdecl)]
    public extern static CairoHandle Create(SurfaceHandle surface);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSourceSurface(this CairoHandle cairo, SurfaceHandle surface, double x, double y);

    // TODO Deprecated
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_surface_create_similar_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static SurfaceHandle SurfaceCreateSimilar(this SurfaceHandle native, CairoContent cairoContent, int width, int height);

    public static CairoHandle SourceRgb(this CairoHandle cairo, double r, double g, double b)
        => cairo.SideEffect(c => c.SetSourceRgb(r, g, b));

    public static CairoHandle SourceRgba(this CairoHandle cairo, double r, double g, double b, double a)
        => cairo.SideEffect(c => c.SetSourceRgba(r, g, b, a));

    public static CairoHandle Paint(this CairoHandle cairo)
        => cairo.SideEffect(c => c._Paint());

    public static CairoHandle Rectangle(this CairoHandle cairo, double x, double y, double width, double height)
        => cairo.SideEffect(c => c._Rectangle(x, y, width, height));

    public static CairoHandle Fill(this CairoHandle cairo)
        => cairo.SideEffect(c => c._Fill());

    public static CairoHandle AntiAlias(this CairoHandle cairo, CairoAntialias antialias)
        => cairo.SideEffect(c => c.SetAntiAlias(antialias));

    public static CairoHandle LineJoin(this CairoHandle cairo, LineJoin lineJoin)
        => cairo.SideEffect(c => c.SetLineJoin(lineJoin));

    public static CairoHandle LineCap(this CairoHandle cairo, LineCap lineCap)
        => cairo.SideEffect(c => c.SetLineCap(lineCap));

    public static CairoHandle LineWidth(this CairoHandle cairo, double width)
        => cairo.SideEffect(c => c.SetLineWidth(width));

    public static CairoHandle Translate(this CairoHandle cairo, double x, double y)
        => cairo.SideEffect(c => c._Translate(x, y));

    public static CairoHandle Stroke(this CairoHandle cairo)
        => cairo.SideEffect(c => c._Stroke());

    public static CairoHandle StrokePreserve(this CairoHandle cairo)
        => cairo.SideEffect(c => c._StrokePreserve());

    public static CairoHandle ArcNegative(this CairoHandle cairo, double x, double y, double radius, double angle1, double angle2)
        => cairo.SideEffect(c => c._ArcNegative(x, y, radius, angle1, angle2));

    public static CairoHandle LineTo(this CairoHandle cairo, double x, double y)
        => cairo.SideEffect(c => c._LineTo(x, y));

    public static CairoHandle MoveTo(this CairoHandle cairo, double x, double y)
        => cairo.SideEffect(c => c._MoveTo(x, y));

    public static CairoHandle Arc(this CairoHandle cairo, double x, double y, double radius, double angle1, double angle2)
        => cairo.SideEffect(c => c._Arc(x, y, radius, angle1, angle2));

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_surface_destroy", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SurfaceDestroy(this IntPtr surface);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_destroy", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Destroy(this IntPtr cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_rgb", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSourceRgb(this CairoHandle cairo, double r, double g, double b);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_rgba", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSourceRgba(this CairoHandle cairo, double r, double g, double b, double a);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_paint", CallingConvention = CallingConvention.Cdecl)]
    extern static CairoHandle _Paint(this CairoHandle cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_rectangle", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Rectangle(this CairoHandle cairo, double x, double y, double width, double height);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_fill", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Fill(this CairoHandle cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_antialias", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetAntiAlias(this CairoHandle cairo, CairoAntialias antialias);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_join", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLineJoin(this CairoHandle cairo, LineJoin lineJoin);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_cap", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLineCap(this CairoHandle cairo, LineCap lineCap);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_width", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLineWidth(this CairoHandle cairo, double w);
    
    [DllImport(Libs.LibGtk, EntryPoint = "cairo_translate", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Translate(this CairoHandle cairo, double x, double y);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_stroke", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Stroke(this CairoHandle cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_stroke_preserve", CallingConvention = CallingConvention.Cdecl)]
    extern static void _StrokePreserve(this CairoHandle cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_arc_negative", CallingConvention = CallingConvention.Cdecl)]
    extern static void _ArcNegative(this CairoHandle cairo, double x, double y, double radius, double angle1, double angle2);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_line_to", CallingConvention = CallingConvention.Cdecl)]
    extern static void _LineTo(this CairoHandle cairo, double x, double y);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_move_to", CallingConvention = CallingConvention.Cdecl)]
    extern static void _MoveTo(this CairoHandle cairo, double x, double y);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_arc", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Arc(this CairoHandle cairo, double x, double y, double radius, double angle1, double angle2);
}

