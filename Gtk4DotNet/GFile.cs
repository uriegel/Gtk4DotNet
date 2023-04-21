using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace GtkDotNet;

public class GFile
{
    /// <summary>
    /// Creates a new GFile object. Free it with GObject.Unref
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    [DllImport(Globals.LibGtk, EntryPoint = "g_file_new_for_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string path);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_trash", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Trash(IntPtr file, IntPtr cancellable, ref IntPtr error);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_copy", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Copy(IntPtr source, IntPtr destination, FileCopyFlags flags, IntPtr cancellable, FileProgressCallback progress, IntPtr data, ref IntPtr error);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_move", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Move(IntPtr source, IntPtr destination, FileCopyFlags flags, IntPtr cancellable, FileProgressCallback progress, IntPtr data, ref IntPtr error);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_get_basename", CallingConvention = CallingConvention.Cdecl)]
    public extern static string GetBasename(IntPtr file);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static string GetPath(IntPtr file);
    
    public static (IntPtr content, long length)? LoadContents(IntPtr file)
    {
        var result = LoadContents(file, IntPtr.Zero, out var content, out var length, IntPtr.Zero, IntPtr.Zero);    
        if (result)
            return (content, length);
        else
            return null;
    }

    public static Task<IEnumerable<IntPtr>> EnumerateChildrenAsync(IntPtr file, string attributes, FileQueryInfoFlags flags, int ioPriority)
    {
        var tcs = new TaskCompletionSource<IEnumerable<IntPtr>>();
        var id = Interlocked.Increment(ref asyncReadyId);
        var delegat = new AsyncReadyCallback(AsyncReady);
        asyncReadyCallbacks[id] = delegat;

        EnumerateChildrenAsync(file, attributes, flags, ioPriority, IntPtr.Zero, delegat, IntPtr.Zero);    
        return tcs.Task;

        void AsyncReady(IntPtr source, IntPtr result, IntPtr zero)
        {
            asyncReadyCallbacks.Remove(id, out var _);
            var enumerator = GFile.EnumerateChildrenFinish(file, result, IntPtr.Zero);
            var items = GetItems().ToArray();
            tcs.TrySetResult(items);

            IEnumerable<IntPtr> GetItems()
            {
                IntPtr next;
                while ((next = GFileEnumerator.NextFile(enumerator, IntPtr.Zero, IntPtr.Zero)) != IntPtr.Zero)
                    yield return next;
                GObject.Unref(enumerator);
            }
        }
    }

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_enumerate_children_async", CallingConvention = CallingConvention.Cdecl)]   
    extern static bool EnumerateChildrenAsync(IntPtr file, string attributes, FileQueryInfoFlags flags, int ioPriority, IntPtr cancellable, AsyncReadyCallback cb, IntPtr zero);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_enumerate_children_finish", CallingConvention = CallingConvention.Cdecl)]   
    extern static IntPtr EnumerateChildrenFinish(IntPtr file, IntPtr asyncResult, IntPtr error);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_load_contents", CallingConvention = CallingConvention.Cdecl)]
    extern static bool LoadContents(IntPtr file, IntPtr cancellable, out IntPtr content, out int length, IntPtr etagOut, IntPtr error);
    
    public delegate void FileProgressCallback(long current, long total, IntPtr zero);
    public delegate void AsyncReadyCallback(IntPtr source, IntPtr result, IntPtr zero);

    static int asyncReadyId = 0;
    static ConcurrentDictionary<int, AsyncReadyCallback> asyncReadyCallbacks = new();
}
