using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class DrawingArea
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_drawing_area_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    public static void SetDrawFunction(IntPtr drawingArea, DrawFunction drawFunction) 
        => SetDrawFunction(drawingArea, drawFunction, IntPtr.Zero, p => {});
    public delegate void DrawFunction(IntPtr drawingArea, IntPtr cairo, int width, int height, IntPtr data);
    delegate void OnDestroyFunction(IntPtr nil);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_drawing_area_set_draw_func", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetDrawFunction(IntPtr drawingArea, DrawFunction drawFunction, IntPtr zero, OnDestroyFunction onDestroy);
}

