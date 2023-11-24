using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class DrawingArea
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static DrawingAreaHandle New();

    public static DrawingAreaHandle SetDrawFunction(this DrawingAreaHandle drawingArea, Action<DrawingAreaHandle, CairoWeakHandle, int, int> draw)
    {
        void drawFunction(IntPtr _, IntPtr cairo, int w, int h, IntPtr ___)  => draw(drawingArea, CairoWeakHandle.Create(cairo), w, h);
        drawingArea.SetDrawFunction(Marshal.GetFunctionPointerForDelegate((DrawFunctionDelegate)drawFunction), IntPtr.Zero, p => { });
        return drawingArea;
    }

    public static DrawingAreaHandle OnResize(this DrawingAreaHandle da, Action<DrawingAreaHandle, int, int> resize)
        => da.SideEffect(a => Gtk.SignalConnect<DrawingAreaResizeDelegate>(a, "resize", 
            (IntPtr drawingArea, int width, int height, IntPtr data) => resize(da, width, height)));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_set_draw_func", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDrawFunction(this DrawingAreaHandle drawingArea, IntPtr drawFunction, IntPtr zero, OnePointerDelegate onDestroy);
}
