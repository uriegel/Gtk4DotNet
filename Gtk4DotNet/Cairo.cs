using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Cairo
{
    [DllImport(Globals.LibGtk, EntryPoint = "cairo_create", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Create(IntPtr surface);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_paint", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Paint(IntPtr cairo);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_set_antialias", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetAntiAlias(IntPtr context, CairoAntialias antialias);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_set_line_join", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetLineJoin(IntPtr context, LineJoin lineJoin);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_set_line_cap", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetLineCap(IntPtr context, LineCap lineCap);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_translate", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Translate(IntPtr context, double x, double y);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_stroke_preserve", CallingConvention = CallingConvention.Cdecl)]
    public extern static void StrokePreserve(IntPtr context);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_arc_negative", CallingConvention = CallingConvention.Cdecl)]
    public extern static void ArcNegative(IntPtr context, double x, double y, double radius, double angle1, double angle2);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_line_to", CallingConvention = CallingConvention.Cdecl)]
    public extern static void LineTo(IntPtr context, double x, double y);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_rectangle", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Rectangle(IntPtr context, double x, double y, double width, double height);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_set_source_rgb", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSourceRgb(IntPtr context, double r, double g, double b);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_fill", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Fill(IntPtr context);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_move_to", CallingConvention = CallingConvention.Cdecl)]
    public extern static void MoveTo(IntPtr context, double x, double y);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_arc", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Arc(IntPtr context, double x, double y, double radius, double angle1, double angle2);

    [DllImport(Globals.LibGtk, EntryPoint = "gdk_surface_create_similar_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr SurfaceCreateSimilar(IntPtr surface, CairoContent cairoContent, int width, int height);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_surface_destroy", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SurfaceDestroy(IntPtr surface);

    [DllImport(Globals.LibGtk, EntryPoint = "cairo_set_source_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSourceSurface(IntPtr cairo,  IntPtr surface, double x, double y);
    
    [DllImport(Globals.LibGtk, EntryPoint = "cairo_destroy", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Destroy(IntPtr cairo);

}

