using System.Runtime.InteropServices;
using Gtk4DotNet.Exceptions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class GFile : GObject
{
    public string? Path
    {
        get => GetPath(this).PtrToString(true);
    }

    public static GFile New(string path)
    {
        var file = _New(path);
        file.CheckDiagnostics();
        return file;
    }

    public string? GetBasename() => GetBasename(this).PtrToString(true);

    public string? LoadStringContents()
    {
        var result = LoadContents(this, Cancellable.Zero(), out var content, out var length, IntPtr.Zero, IntPtr.Zero);
        return result
            ? content.PtrToString(true) ?? ""
            : null;
    }

    public Task TrashAsync()
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        Trash(this, 100, Cancellable.Zero(), asyncReady, 0);
        return tcs.Task;

        void AsyncReadyCallback(nint _, nint result, nint __)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            nint error = 0;
            if (TrashFinish(this, result, ref error))
                tcs.TrySetResult();
            else
            {
                var gerror = new GErrorStruct(error);
                tcs.TrySetException(new GFileException(Path, gerror));
            }
        }
    }

    public GFileInfo QueryContentType() => QueryInfo("standard::content-type");

    public GFileInfo QueryInfo(string attributes)
    {
        var info = _QueryInfo(this, attributes, 0, 0, 0);
        info.CheckDiagnostics();
        return info;
    } 

    public Task CopyAsync(string destination, FileCopyFlags flags = FileCopyFlags.None,
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var destinationFile = New(destination);
        var rcb = cb != null ? new TwoLongAndPtrCallback((c, t, _) => cb(c, t)) : null;
        if (rcb != null)
            AsyncReady.ProgressCallbacks[id] = rcb;
        cb?.Invoke(0, 0);
        CopyAsync(this, destinationFile, flags, 100, cancellable?.IsInvalid == false ? cancellable : Cancellable.Zero(), rcb, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, IntPtr zero)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            AsyncReady.ProgressCallbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            var res = CopyFinish(this, result, ref error);
            if (res)
                tcs.TrySetResult();
            else
            {
                var gerror = new GErrorStruct(error);
                if (createTargetPath && gerror.Domain == 232 && gerror.Code == 1 && File.Exists(Path))
                {
                    var fi = new FileInfo(destination);
                    var destPath = fi.Directory;
                    try
                    {
                        destPath?.Create();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        tcs.TrySetException(new GFileException(Path, new GErrorStruct(232, 14, "Access Denied")));
                    }
                    catch
                    {
                        tcs.TrySetException(new GFileException(Path, new GErrorStruct(0, 0, "General Exception")));
                    }

                    await CopyAsync(destination, flags, true, cb, cancellation);
                }
                else
                    tcs.TrySetException(new GFileException(Path, gerror));
            }
        }
    }

    public Task MoveAsync(string destination, FileCopyFlags flags = FileCopyFlags.None,
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        using var cancellable = cancellation.HasValue ? new Cancellable(cancellation.Value) : null;
        using var destinationFile = New(destination);
        var rcb = cb != null ? new TwoLongAndPtrCallback((c, t, _) => cb(c, t)) : null;
        if (rcb != null)
            AsyncReady.ProgressCallbacks[id] = rcb;
        cb?.Invoke(0, 0);
        MoveAsync(this, destinationFile, flags, 100, cancellable?.IsInvalid == false ? cancellable : Cancellable.Zero(), rcb, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, nint zero)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            AsyncReady.ProgressCallbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            var res = MoveFinish(this, result, ref error);
            if (res)
                tcs.TrySetResult();
            else
            {
                var gerror = new GErrorStruct(error);
                if (createTargetPath && gerror.Domain == 232 && gerror.Code == 1 && File.Exists(Path))
                {
                    var fi = new FileInfo(destination);
                    var destPath = fi.Directory;
                    try
                    {
                        destPath?.Create();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        tcs.TrySetException(new GFileException(Path, new GErrorStruct(232, 14, "Access Denied")));
                    }
                    catch
                    {
                        tcs.TrySetException(new GFileException(Path, new GErrorStruct(0, 0, "General Exception")));
                    }

                    await MoveAsync(destination, flags, true, cb, cancellation);
                }
                else
                    tcs.TrySetException(new GFileException(Path, gerror));
            }
        }
    }

    // public MountHandle FindEnclosingMount() 
    //     => FindEnclosingMount(this, 0, 0);

    public bool CopyAttributes(GFile target, FileCopyFlags flags)
        => CopyAttributes(this, target, flags, 0, 0);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_new_for_path", CallingConvention = CallingConvention.Cdecl)]
    extern static GFile _New(string path);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_load_contents", CallingConvention = CallingConvention.Cdecl)]
    extern static bool LoadContents(GFile gFile, Cancellable cancellable, out IntPtr content, out int length, IntPtr etagOut, IntPtr error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_async", CallingConvention = CallingConvention.Cdecl)]
    extern static void CopyAsync(GFile source, GFile destination, FileCopyFlags flags, int priority, Cancellable cancellable,
        TwoLongAndPtrCallback? progress, nint _, ThreePointerDelegate asyncCallback, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool CopyFinish(GFile source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_move_async", CallingConvention = CallingConvention.Cdecl)]
    extern static void MoveAsync(GFile source, GFile destination, FileCopyFlags flags, int priority, Cancellable cancellable,
        TwoLongAndPtrCallback? progress, nint _, ThreePointerDelegate asyncCallback, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_move_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool MoveFinish(GFile source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_trash_async", CallingConvention = CallingConvention.Cdecl)]
    extern static bool Trash(GFile file, int prio, Cancellable cancellable, ThreePointerDelegate asyncCallback, nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_trash_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool TrashFinish(GFile source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_basename", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetBasename(GFile file);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_get_path", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetPath(GFile file);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_attributes", CallingConvention = CallingConvention.Cdecl)]
    extern static bool CopyAttributes(GFile file, GFile starget, FileCopyFlags flags, nint nil, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_query_info", CallingConvention = CallingConvention.Cdecl)]
    extern static GFileInfo _QueryInfo(GFile file, string attributes, int flags, nint nil, nint nil2);

    // [DllImport(Libs.LibGtk, EntryPoint = "g_file_find_enclosing_mount", CallingConvention = CallingConvention.Cdecl)]
    // extern static MountHandle FindEnclosingMount(GFile file, nint _, nint __);
}
