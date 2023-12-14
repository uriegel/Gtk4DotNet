using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using CsTools.Functional;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

using static CsTools.Core;

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
        
    public static Result<Nothing, GError> Copy(this GFileHandle source, string destination, FileCopyFlags flags, ProgressCallback? cb)
        => Copy(source, destination, flags, false, cb);

    public static Result<Nothing, GError> Copy(this GFileHandle source, string destination, FileCopyFlags flags, bool createTargetPath = false, 
        ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var destinationFile = New(destination);
        var error = IntPtr.Zero;
        TwoLongAndPtrCallback? rcb = cb != null ? (c, t, _) => cb(c, t) : null;
        cb?.Invoke(0, 0);
        if (!Copy(source, destinationFile, flags, cancellable?.handle?.IsInvalid == false ? cancellable.handle : Cancellable.Zero().handle, rcb, IntPtr.Zero, ref error))
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
                    return Error<Nothing, GError>(GError.New(new GErrorStruct(232, 14, "Access Denied"), path));
                }
                catch 
                {
                    return Error<Nothing, GError>(GError.New(new GErrorStruct(0, 0, "General Exception"), path));
                }
                Copy(source, destination, flags, true, cb, cancellation);
                return nothing;
            }
            else
            {
                if (gerror.Code == 19 && gerror.Domain == 236)
                {
                    try
                    {
                        if (path != null)
                            File.Delete(destination);
                    }
                    catch { }
                }
                return Error<Nothing, GError>(GError.New(gerror, path ?? ""));
            }
        }
        return nothing;
    }

    public static Result<Nothing, GError> Move(this GFileHandle source, string destination, FileCopyFlags flags, ProgressCallback? cb)
        => Move(source, destination, flags, false, cb);

    public static Result<Nothing, GError> Move(this GFileHandle source, string destination, FileCopyFlags flags, bool createTargetPath = false, 
        ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var destinationFile = New(destination);
        var error = IntPtr.Zero;
        TwoLongAndPtrCallback? rcb = cb != null ? (c, t, _) => cb(c, t) : null;
        cb?.Invoke(0, 0);
        if (!Copy(source, destinationFile, flags, cancellable?.handle?.IsInvalid == false ? cancellable.handle : Cancellable.Zero().handle, rcb, IntPtr.Zero, ref error))
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
                    return Error<Nothing, GError>(GError.New(new GErrorStruct(232, 14, "Access Denied"), path));
                }
                catch 
                {
                    return Error<Nothing, GError>(GError.New(new GErrorStruct(0, 0, "General Exception"), path));
                }
                Copy(source, destination, flags, true, cb, cancellation);
                return nothing;
            }
            else
            {
                if (gerror.Code == 19 && gerror.Domain == 236)
                {
                    try
                    {
                        if (path != null)
                            File.Delete(destination);
                    }
                    catch { }
                }
                return Error<Nothing, GError>(GError.New(gerror, path ?? ""));
            }
        }
        return nothing;
    }

    public static Task<Result<Nothing, GError>> CopyAsync(this GFileHandle source, string destination, FileCopyFlags flags, ProgressCallback? cb)
        => CopyAsync(source, destination, flags, 100, false, cb);

    public static Task<Result<Nothing, GError>> CopyAsync(this GFileHandle source, string destination, FileCopyFlags flags, int ioPriority = 100, 
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        var tcs = new TaskCompletionSource<Result<Nothing, GError>>();
        var id = getId();
        var asyncReady = new ThreePointerDelegate(AsyncReady);
        asyncReadyCallbacks[id] = asyncReady;
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var destinationFile = New(destination);
        TwoLongAndPtrCallback? rcb = cb != null ? (c, t, _) => cb(c, t) : null;
        cb?.Invoke(0, 0);
        CopyAsync(source, destinationFile, flags, ioPriority, cancellable?.handle?.IsInvalid == false ? cancellable.handle : Cancellable.Zero().handle, rcb, IntPtr.Zero, asyncReady, IntPtr.Zero);
        return tcs.Task;

        async void AsyncReady(IntPtr _, IntPtr result, IntPtr zero)
        {     
            asyncReadyCallbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            var res = CopyFinish(source, result, ref error);
            if (res)
                tcs.TrySetResult(nothing);
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
                        tcs.TrySetResult(Error<Nothing, GError>(GError.New(new GErrorStruct(232, 14, "Access Denied"), path)));
                    }
                    catch
                    {
                        tcs.TrySetResult(Error<Nothing, GError>(GError.New(new GErrorStruct(0, 0, "General Exception"), path)));
                    }

                    // TODO try again
                    await CopyAsync(source, destination, flags, ioPriority, true, cb, cancellation);
                    //return 0;
                }
                else
                    tcs.TrySetResult(Error<Nothing, GError>(GError.New(gerror, path ?? "")));
            }
        }
    }

    public static Result<Nothing, GError> Trash(this GFileHandle file)
    {
        var error = IntPtr.Zero;
        return Trash(file, Cancellable.Zero().handle, ref error)
            ? nothing
            : Error<Nothing, GError>(GError.New(new GErrorStruct(error), file.GetPath() ?? ""));
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy", CallingConvention = CallingConvention.Cdecl)]
    extern static bool Copy(GFileHandle source, GFileHandle destination, FileCopyFlags flags, CancellableHandle cancellable, 
        TwoLongAndPtrCallback? progress, IntPtr data, ref IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_async", CallingConvention = CallingConvention.Cdecl)]
    extern static void CopyAsync(GFileHandle source, GFileHandle destination, FileCopyFlags flags, int priority, CancellableHandle cancellable, 
        TwoLongAndPtrCallback? progress, IntPtr data, ThreePointerDelegate asyncCallback, IntPtr zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool CopyFinish(GFileHandle source, IntPtr asyncResult, ref IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_move", CallingConvention = CallingConvention.Cdecl)]
    extern static bool Move(IntPtr source, IntPtr destination, FileCopyFlags flags, CancellableHandle cancellable, 
        TwoLongAndPtrCallback progress, IntPtr data, ref IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_load_contents", CallingConvention = CallingConvention.Cdecl)]
    extern static bool LoadContents(this GFileHandle gFile, CancellableHandle cancellable, out IntPtr content, out int length, IntPtr etagOut, IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_trash", CallingConvention = CallingConvention.Cdecl)]
    extern static bool Trash(this GFileHandle file, CancellableHandle cancellable, ref IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_basename", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetBasename(this GFileHandle file);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetPath(this GFileHandle file);

    readonly static Func<int> getId = Incrementor.UseInt();
    readonly static ConcurrentDictionary<int, ThreePointerDelegate> asyncReadyCallbacks = new();
}