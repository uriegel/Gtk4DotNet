using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class Cairo : GObject
{
    public Cairo(bool isWeak = false) : base() => this.isWeak = isWeak;

    internal static Cairo CreateWeak(nint raw)
    {
        return new Cairo(true)
        {
            handle = raw,
            
        };
    }

    protected override bool ReleaseHandle()
    {
        if (!isWeak)
            Destroy(handle);
        return true;
    }
                
    [DllImport(Libs.LibGtk, EntryPoint = "cairo_create", CallingConvention = CallingConvention.Cdecl)]
    public extern static Cairo Create(Surface surface);

    public Cairo SourceRgb(double r, double g, double b)
        => this.SideEffect(c => SetSourceRgb(this, r, g, b));

    public Cairo SourceRgba(double r, double g, double b, double a)
        => this.SideEffect(c => SetSourceRgba(this, r, g, b, a));

    public Cairo Paint()
        => this.SideEffect(c => Paint(this));

    public Cairo Rectangle(double x, double y, double width, double height)
        => this.SideEffect(c => Rectangle(this, x, y, width, height));

    public Cairo Fill()
        => this.SideEffect(c => Fill(this));

    public Cairo AntiAlias(CairoAntialias antialias)
        => this.SideEffect(c => SetAntiAlias(this, antialias));

    public Cairo LineJoin(LineJoin lineJoin)
        => this.SideEffect(c => SetLineJoin(this, lineJoin));

    public Cairo LineCap(LineCap lineCap)
        => this.SideEffect(c => SetLineCap(this, lineCap));

    public Cairo LineWidth(double width)
        => this.SideEffect(c => SetLineWidth(this, width));

    public Cairo Translate(double x, double y)
        => this.SideEffect(c => Translate(this, x, y));

    public Cairo Stroke()
        => this.SideEffect(c => Stroke(this));

    public Cairo StrokePreserve()
        => this.SideEffect(c => StrokePreserve(this));

    public Cairo ArcNegative(double x, double y, double radius, double angle1, double angle2)
        => this.SideEffect(c => ArcNegative(this, x, y, radius, angle1, angle2));

    public Cairo LineTo(double x, double y)
        => this.SideEffect(c => LineTo(this, x, y));

    public Cairo MoveTo(double x, double y)
        => this.SideEffect(c => MoveTo(this, x, y));

    public Cairo Arc(double x, double y, double radius, double angle1, double angle2)
        => this.SideEffect(c => Arc(this, x, y, radius, angle1, angle2));

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_destroy", CallingConvention = CallingConvention.Cdecl)]
    extern static void Destroy(nint cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_surface", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSourceSurface(Cairo cairo, Surface surface, double x, double y);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_rgb", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSourceRgb(Cairo cairo, double r, double g, double b);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_source_rgba", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSourceRgba(Cairo cairo, double r, double g, double b, double a);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_paint", CallingConvention = CallingConvention.Cdecl)]
    extern static void Paint(Cairo cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_rectangle", CallingConvention = CallingConvention.Cdecl)]
    extern static void Rectangle(Cairo cairo, double x, double y, double width, double height);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_fill", CallingConvention = CallingConvention.Cdecl)]
    extern static void Fill(Cairo cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_antialias", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetAntiAlias(Cairo cairo, CairoAntialias antialias);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_join", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLineJoin(Cairo cairo, LineJoin lineJoin);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_cap", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLineCap(Cairo cairo, LineCap lineCap);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_set_line_width", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetLineWidth(Cairo cairo, double w);
    
    [DllImport(Libs.LibGtk, EntryPoint = "cairo_translate", CallingConvention = CallingConvention.Cdecl)]
    extern static void Translate(Cairo cairo, double x, double y);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_stroke", CallingConvention = CallingConvention.Cdecl)]
    extern static void Stroke(Cairo cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_stroke_preserve", CallingConvention = CallingConvention.Cdecl)]
    extern static void StrokePreserve(Cairo cairo);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_arc_negative", CallingConvention = CallingConvention.Cdecl)]
    extern static void ArcNegative(Cairo cairo, double x, double y, double radius, double angle1, double angle2);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_line_to", CallingConvention = CallingConvention.Cdecl)]
    extern static void LineTo(Cairo cairo, double x, double y);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_move_to", CallingConvention = CallingConvention.Cdecl)]
    extern static void MoveTo(Cairo cairo, double x, double y);

    [DllImport(Libs.LibGtk, EntryPoint = "cairo_arc", CallingConvention = CallingConvention.Cdecl)]
    extern static void Arc(Cairo cairo, double x, double y, double radius, double angle1, double angle2);

    readonly bool isWeak;
}

