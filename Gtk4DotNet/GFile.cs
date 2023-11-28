using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GFile 
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_file_new_for_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static GFileHandle New(string path);

    public static string? GetBasename(this GFileHandle file)
        => file._GetBasename().PtrToString(true);

    public static string? GetPath(this GFileHandle file)
        => file._GetPath().PtrToString(true);

    public static string? LoadStringContents(this GFileHandle file)
    {
        var result = LoadContents(file, IntPtr.Zero, out var content, out var length, IntPtr.Zero, IntPtr.Zero);
        return result
            ? content.PtrToString(true) ?? ""
            : null;
    }
        
    public static void Trash(this GFileHandle file)
    {
        var error = IntPtr.Zero;
        if (!Trash(file, IntPtr.Zero, ref error))
            throw GException.New(new GError(error), file.GetPath() ?? "", "");
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_load_contents", CallingConvention = CallingConvention.Cdecl)]
    extern static bool LoadContents(this GFileHandle gFile, IntPtr cancellable, out IntPtr content, out int length, IntPtr etagOut, IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_trash", CallingConvention = CallingConvention.Cdecl)]
    extern static bool Trash(this GFileHandle file, IntPtr cancellable, ref IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_basename", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetBasename(this GFileHandle file);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetPath(this GFileHandle file);
}