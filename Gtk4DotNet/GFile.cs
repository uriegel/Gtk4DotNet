using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using CsTools.Functional;
using GtkDotNet.Exceptions;
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
        var result = LoadContents(file, Cancellable.Zero().handle, out var content, out var length, IntPtr.Zero, IntPtr.Zero);
        return result
            ? content.PtrToString(true) ?? ""
            : null;
    }
        
    public static Task TrashAsync(this GFileHandle file)
    {
        var tcs = new TaskCompletionSource();
        var id = getId();
        var asyncReady = new ThreePointerDelegate(AsyncReady);
        asyncReadyCallbacks[id] = asyncReady;
        Trash(file, 100, Cancellable.Zero().handle, asyncReady, 0);
        return tcs.Task;

        void AsyncReady(nint _, nint result, nint __)
        {     
            asyncReadyCallbacks.Remove(id, out var _);
            nint error = 0;
            if (TrashFinish(file, result, ref error))
                tcs.TrySetResult();
            else
            {
                var gerror = new GErrorStruct(error);
                var path = file.GetPath();
                tcs.TrySetException(new GFileException(path, gerror));
            }
        }
    }

    public static Task CopyAsync(this GFileHandle source, string destination, FileCopyFlags flags= FileCopyFlags.None, 
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        var tcs = new TaskCompletionSource();
        var id = getId();
        var asyncReady = new ThreePointerDelegate(AsyncReady);
        asyncReadyCallbacks[id] = asyncReady;
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var destinationFile = New(destination);
        TwoLongAndPtrCallback? rcb = cb != null ? (c, t, _) => cb(c, t) : null;
        cb?.Invoke(0, 0);
        CopyAsync(source, destinationFile, flags, 100, cancellable?.handle?.IsInvalid == false ? cancellable.handle : Cancellable.Zero().handle, rcb, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReady(IntPtr _, IntPtr result, IntPtr zero)
        {     
            asyncReadyCallbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            var res = CopyFinish(source, result, ref error);
            if (res)
                tcs.TrySetResult();
            else
            {
                var gerror = new GErrorStruct(error);
                var path = source.GetPath();
                if (createTargetPath && gerror.Domain == 232 && gerror.Code == 1 && File.Exists(path))
                {
                    var fi = new FileInfo(destination);
                    var destPath = fi.Directory;
                    try
                    {
                        destPath?.Create();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        tcs.TrySetException(new GFileException(path, new GErrorStruct(232, 14, "Access Denied")));
                    }
                    catch
                    {
                        tcs.TrySetException(new GFileException(path, new GErrorStruct(0, 0, "General Exception")));
                    }

                    await CopyAsync(source, destination, flags, true, cb, cancellation);
                }
                else
                    tcs.TrySetException(new GFileException(path,gerror));
            }
        }
    }

    public static Task MoveAsync(this GFileHandle source, string destination, FileCopyFlags flags= FileCopyFlags.None, 
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        var tcs = new TaskCompletionSource();
        var id = getId();
        var asyncReady = new ThreePointerDelegate(AsyncReady);
        asyncReadyCallbacks[id] = asyncReady;
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var destinationFile = New(destination);
        TwoLongAndPtrCallback? rcb = cb != null ? (c, t, _) => cb(c, t) : null;
        cb?.Invoke(0, 0);
        MoveAsync(source, destinationFile, flags, 100, cancellable?.handle?.IsInvalid == false ? cancellable.handle : Cancellable.Zero().handle, rcb, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReady(IntPtr _, IntPtr result, IntPtr zero)
        {     
            asyncReadyCallbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            var res = MoveFinish(source, result, ref error);
            if (res)
                tcs.TrySetResult();
            else
            {
                var gerror = new GErrorStruct(error);
                var path = source.GetPath();
                if (createTargetPath && gerror.Domain == 232 && gerror.Code == 1 && File.Exists(path))
                {
                    var fi = new FileInfo(destination);
                    var destPath = fi.Directory;
                    try
                    {
                        destPath?.Create();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        tcs.TrySetException(new GFileException(path, new GErrorStruct(232, 14, "Access Denied")));
                    }
                    catch
                    {
                        tcs.TrySetException(new GFileException(path, new GErrorStruct(0, 0, "General Exception")));
                    }

                    await MoveAsync(source, destination, flags, true, cb, cancellation);
                }
                else
                    tcs.TrySetException(new GFileException(path,gerror));
            }
        }
    }

    public static bool CopyAttributes(this GFileHandle file, GFileHandle starget, FileCopyFlags flags)
        => _CopyAttributes(file, starget, flags, 0, 0);


    [DllImport(Libs.LibGtk, EntryPoint = "g_file_load_contents", CallingConvention = CallingConvention.Cdecl)]
    extern static bool LoadContents(this GFileHandle gFile, CancellableHandle cancellable, out IntPtr content, out int length, IntPtr etagOut, IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_async", CallingConvention = CallingConvention.Cdecl)]
    extern static void CopyAsync(GFileHandle source, GFileHandle destination, FileCopyFlags flags, int priority, CancellableHandle cancellable, 
        TwoLongAndPtrCallback? progress, nint _, ThreePointerDelegate asyncCallback, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool CopyFinish(GFileHandle source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_move_async", CallingConvention = CallingConvention.Cdecl)]
    extern static void MoveAsync(GFileHandle source, GFileHandle destination, FileCopyFlags flags, int priority, CancellableHandle cancellable, 
        TwoLongAndPtrCallback? progress, nint _, ThreePointerDelegate asyncCallback, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_move_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool MoveFinish(GFileHandle source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_trash_async", CallingConvention = CallingConvention.Cdecl)]
    extern static bool Trash(this GFileHandle file, int prio, CancellableHandle cancellable, ThreePointerDelegate asyncCallback, nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_trash_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool TrashFinish(GFileHandle source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_basename", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetBasename(this GFileHandle file);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetPath(this GFileHandle file);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_attributes", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _CopyAttributes(this GFileHandle file, GFileHandle starget, FileCopyFlags flags, nint nil, nint nil2);

    readonly static Func<int> getId = Incrementor.UseInt();

    internal static int GetAsyncReadyDelegates() => asyncReadyCallbacks.Count;

    readonly static ConcurrentDictionary<int, ThreePointerDelegate> asyncReadyCallbacks = new();
}