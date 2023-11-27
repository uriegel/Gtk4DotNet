using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GFile 
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_file_new_for_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static GFileHandle New(string path);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_basename", CallingConvention = CallingConvention.Cdecl)]
    public extern static string GetBasename(this GFileHandle file);

    public static string? LoadStringContents(this GFileHandle file)
    {
        var result = LoadContents(file, IntPtr.Zero, out var content, out var length, IntPtr.Zero, IntPtr.Zero);
        return result
            ? content.PtrToString(true) ?? ""
            : null;
    }
        

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_load_contents", CallingConvention = CallingConvention.Cdecl)]
    extern static bool LoadContents(this GFileHandle gFile, IntPtr cancellable, out IntPtr content, out int length, IntPtr etagOut, IntPtr error);


   // public static DrawingAreaHandle SetDrawFunction(this DrawingAreaHandle drawingArea, Action<DrawingAreaHandle, CairoWeakHandle, int, int> draw)
    // {
    //     void drawFunction(IntPtr _, IntPtr cairo, int w, int h, IntPtr ___)  => draw(drawingArea, CairoWeakHandle.Create(cairo), w, h);
    //     drawingArea.SetDrawFunction(Marshal.GetFunctionPointerForDelegate((DrawFunctionDelegate)drawFunction), IntPtr.Zero, p => { });
    //     return drawingArea;
    // }
}