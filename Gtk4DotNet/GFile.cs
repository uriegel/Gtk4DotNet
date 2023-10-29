using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using LinqTools;

namespace GtkDotNet;

public static class GFile
{
    /// <summary>
    /// Creates a new GFile object. Free it with GObject.Unref or use GObjectRef with using
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    [DllImport(Globals.LibGtk, EntryPoint = "g_file_new_for_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string path);

    public static void Copy(string source, string destination, FileCopyFlags flags, ProgressCallback cb)
        => Copy(source, destination, flags, false, cb);

    public static void Copy(string source, string destination, FileCopyFlags flags, bool createTargetPath, ProgressCallback cb)            
        => Copy(source, destination, flags, createTargetPath, cb, null);

    public static void Copy(string source, string destination, FileCopyFlags flags, bool createTargetPath, ProgressCallback cb, CancellationToken? cancellation)
    {
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var sourceFile = GObjectRef.WithRef(New(source));
        using var destinationFile = GObjectRef.WithRef(New(destination));
        var error = IntPtr.Zero;
        FileProgressCallback rcb = cb != null ? (c, t, _) => cb(c, t) : null;
        cb?.Invoke(0, 0);
        if (!Copy(sourceFile.Value, destinationFile.Value, flags, cancellable?.handle ?? IntPtr.Zero, rcb, IntPtr.Zero, ref error))
        {
            var gerror = new GError(error);
            if (createTargetPath && gerror.Domain == 232 && gerror.Code == 1 && File.Exists(source))
            {
                var fi = new FileInfo(destination);
                var path = fi.Directory;
                try 
                {
                    path.Create();
                }
                catch (UnauthorizedAccessException)
                {
                    throw GErrorException.New(new GError(232, 14, "Access Denied"), source, destination);
                }
                Copy(source, destination, flags, true, cb, cancellation);
            }
            else
                throw GErrorException.New(gerror, source, destination);
        }
    }

        public static void Move(string source, string destination, FileCopyFlags flags, ProgressCallback cb)
            => Move(source, destination, flags, false, cb);

        public static void Move(string source, string destination, FileCopyFlags flags, bool createTargetPath, ProgressCallback cb)            
            => Move(source, destination, flags, createTargetPath, cb, null);

        public static void Move(string source, string destination, FileCopyFlags flags, bool createTargetPath, ProgressCallback cb, CancellationToken? cancellation)
        {
            using var sourceFile = GObjectRef.WithRef(New(source));
            using var destinationFile = GObjectRef.WithRef(New(destination));
            var error = IntPtr.Zero;
            FileProgressCallback rcb = cb != null ? (c, t, _) => cb(c, t) : null;
            cb?.Invoke(0, 0);
            if (!Move(sourceFile.Value, destinationFile.Value, flags, IntPtr.Zero, rcb, IntPtr.Zero, ref error))
            {
                var gerror = new GError(error);
                if (createTargetPath && gerror.Domain == 232 && gerror.Code == 1 && File.Exists(source))
                {
                    var fi = new FileInfo(destination);
                    var path = fi.Directory;
                    try 
                    {
                        path.Create();
                    }
                    catch (AccessDeniedException)
                    {
                        throw GErrorException.New(new GError(232, 14, "Access Denied"), source, destination);
                    }
                    Move(source, destination, flags, true, cb);
                }
                else
                    throw GErrorException.New(gerror, source, destination);
            }
        }

    public static void Trash(string path)
        => GObjectRef
            .WithRef(GFile.New(path))
            .Use(file =>
            {
                var error = IntPtr.Zero;
                if (!GFile.Trash(file, IntPtr.Zero, ref error))
                    throw GErrorException.New(new GError(error), path, null);
            });

    public delegate void ProgressCallback(long current, long total);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_trash", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Trash(this IntPtr file, IntPtr cancellable, ref IntPtr error);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_copy", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Copy(IntPtr source, IntPtr destination, FileCopyFlags flags, IntPtr cancellable, FileProgressCallback progress, IntPtr data, ref IntPtr error);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_move", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Move(IntPtr source, IntPtr destination, FileCopyFlags flags, IntPtr cancellable, FileProgressCallback progress, IntPtr data, ref IntPtr error);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_get_basename", CallingConvention = CallingConvention.Cdecl)]
    public extern static string GetBasename(this IntPtr file);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static string FileGetPath(this IntPtr file);
    
    public static (IntPtr content, long length)? LoadContents(this IntPtr file)
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
