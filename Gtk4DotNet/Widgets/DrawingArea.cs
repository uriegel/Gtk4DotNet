using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class DrawingArea : Widget
{
    public static DrawingArea New()
    {
        var res = _New();
        res.CheckDiagnostics();
        return res;
    }

    public DrawingArea() : base() { }

    public DrawingArea(Builder builder, string? name = null) : base(builder, name) { }
    public DrawingArea(nint obj) : base() => SetInternalHandle(obj);

    public void SetDrawFunction(Action<DrawingArea, Cairo, int, int> draw)
        => SetDrawFunction((nint _, nint cairo, int w, int h, nint ___) => draw(this, Cairo.CreateWeak(cairo), w, h));

    public DrawingArea OnResize(Action<DrawingArea, int, int> resize)
        => this.SideEffect(a => SignalConnect<DrawingAreaResizeDelegate>(
            "resize", (nint drawingArea, int width, int height, IntPtr data) => resize(this, width, height)));

    void SetDrawFunction(DrawFunctionDelegate draw)
    {
        var key = GtkDelegates.GetKey();
        GtkDelegates.Add(key, draw);
        AddWeakRef(() => GtkDelegates.Remove(key));
        SetDrawFunction(this, Marshal.GetFunctionPointerForDelegate((Delegate)draw), IntPtr.Zero, p => { });
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_set_draw_func", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDrawFunction(DrawingArea drawingArea, IntPtr drawFunction, IntPtr zero, OnePointerDelegate onDestroy);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_drawing_area_new", CallingConvention = CallingConvention.Cdecl)]
    extern static DrawingArea _New();
}




