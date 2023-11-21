using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class DrawingArea
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static DrawingAreaHandle New();

    public static DrawingAreaHandle SetDrawFunction(this DrawingAreaHandle drawingArea, Action<DrawingAreaHandle, IntPtr, int, int> draw)
    {
        void drawFunction(IntPtr _, IntPtr cairo, int w, int h, IntPtr ___)  => draw(drawingArea, cairo, w, h);
        drawingArea.SetDrawFunction(Marshal.GetFunctionPointerForDelegate((DrawFunctionDelegate)drawFunction), IntPtr.Zero, p => { });
        return drawingArea;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_set_draw_func", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDrawFunction(this DrawingAreaHandle drawingArea, IntPtr drawFunction, IntPtr zero, OnePointerDelegate onDestroy);
}
