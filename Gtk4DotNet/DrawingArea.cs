using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class DrawingArea
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static DrawingAreaHandle New();

    public static DrawingAreaHandle SetDrawFunction(this DrawingAreaHandle drawingArea, Action<DrawingAreaHandle, CairoWeakHandle, int, int> draw)
        => SetDrawFunction(drawingArea, (IntPtr _, IntPtr cairo, int w, int h, IntPtr ___) => draw(drawingArea, CairoWeakHandle.Create(cairo), w, h));

    static DrawingAreaHandle SetDrawFunction(this DrawingAreaHandle drawingArea, DrawFunctionDelegate draw)
    {
        var key = GtkDelegates.GetKey();
        GtkDelegates.Add(key, draw);
        drawingArea.AddWeakRefRaw(() => GtkDelegates.Remove(key));
        drawingArea.SetDrawFunction(Marshal.GetFunctionPointerForDelegate((Delegate)draw), IntPtr.Zero, p=>{});
        return drawingArea;
    }

    public static DrawingAreaHandle OnResize(this DrawingAreaHandle da, Action<DrawingAreaHandle, int, int> resize)
        => da.SideEffect(a => Gtk.SignalConnect<DrawingAreaResizeDelegate>(a, "resize", 
            (IntPtr drawingArea, int width, int height, IntPtr data) => resize(da, width, height)));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_drawing_area_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();        

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_set_draw_func", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDrawFunction(this DrawingAreaHandle drawingArea, IntPtr drawFunction, IntPtr zero, OnePointerDelegate onDestroy);
}
