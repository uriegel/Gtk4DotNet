using System.Runtime.InteropServices;
using Gtk4DotNet.ErrorHandling;
using Gtk4DotNet.ErrorHandling.ErrorCodes;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class GFile : GObject
{
    public string? Path { get => GetPath(this).PtrToString(true); }

    public bool Exists { get => _Exists(this, 0); }

    public string? GetBasename() => GetBasename(this).PtrToString(true);

    public static GFile New(string path)
    {
        var file = _New(path);
        file.CheckDiagnostics();
        return file;
    }

    public string LoadStringContents()
    {
        var result = LoadContents(this, 0, out var content, out var length, IntPtr.Zero, IntPtr.Zero);
        return result
            ? content.PtrToString(true) ?? ""
            : "";
    }

    public Task TrashAsync()
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        Trash(this, 100, 0, asyncReady, 0);
        return tcs.Task;

        void AsyncReadyCallback(nint _, nint result, nint __)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            nint error = 0;
            if (TrashFinish(this, result, ref error))
                tcs.TrySetResult();
            else
                tcs.TrySetException(GtkException.Get(error, true));
        }
    }

    public GFileInfo QueryContentType() => QueryInfo("standard::content-type");

    public GFileInfo QueryInfo(string attributes)
    {
        var info = _QueryInfo(this, attributes, 0, 0, 0);
        info.CheckDiagnostics();
        return info;
    }

    public Mount? FindEnclosingMount()
    {
        var mount = FindEnclosingMount(this, 0, 0);
        if (mount.IsInvalid)
            return null;
        // Do not call this, because mount is a living reference
        //mount.CheckDiagnostics();
        return mount;
    }


    /// <summary>
    /// Copies a GFile to destination (you have to specify the destination file name!).
    /// W A R N I N G: If you cancel the copy operation, the target file might remain as partial file!
    /// </summary>
    /// <param name="destination">Destination file path (with file name!)</param>
    /// <param name="flags">Copy flags</param>
    /// <param name="createTargetPath">If target path does not exist, dreate it</param>
    /// <param name="cb">Progress callback, current/total bytes </param>
    /// <param name="cancellation">A cancellation token to cancel the operation. W A R N I N G: If you cancel the copy operation, the target file might remain as partial file!</param>
    /// <returns></returns>
    public Task CopyAsync(string destination, FileCopyFlags flags = FileCopyFlags.None,
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
        => CopyAsync(false, destination, flags, createTargetPath, cb, cancellation);

    /// <summary>
    /// Moves a GFile to destination (you have to specify the destination file name!).
    /// W A R N I N G: If you cancel the move operation, the target file might remain as partial file!
    /// </summary>
    /// <param name="destination">Destination file path (with file name!)</param>
    /// <param name="flags">Copy flags</param>
    /// <param name="createTargetPath">If target path does not exist, dreate it</param>
    /// <param name="cb">Progress callback, current/total bytes </param>
    /// <param name="cancellation">A cancellation token to cancel the operation. W A R N I N G: If you cancel the move operation, the target file might remain as partial file!</param>
    /// <returns></returns>
    public Task MoveAsync(string destination, FileCopyFlags flags = FileCopyFlags.None,
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
        => CopyAsync(true, destination, flags, createTargetPath, cb, cancellation);

    // public MountHandle FindEnclosingMount() 
    //     => FindEnclosingMount(this, 0, 0);

    public bool CopyAttributes(GFile target, FileCopyFlags flags)
        => CopyAttributes(this, target, flags, 0, 0);

    async Task CopyAsync(bool move, string destination, FileCopyFlags flags = FileCopyFlags.None,
        bool createTargetPath = false, ProgressCallback? cb = null, CancellationToken? cancellation = null)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        using var cancellable = Cancellable.New(cancellation);
        var destinationFile = New(destination);
        var rcb = cb != null ? new TwoLongAndPtrCallback((c, t, _) => cb(c, t)) : null;
        if (rcb != null)
            AsyncReady.ProgressCallbacks[id] = rcb;
        cb?.Invoke(0, 0);
        if (move)
            MoveAsync(this, destinationFile, flags, 100, cancellation != null ? cancellable.GetInternalHandle() : 0, rcb, 0, asyncReady, 0);
        else
            CopyAsync(this, destinationFile, flags, 100, cancellation != null ? cancellable.GetInternalHandle() : 0, rcb, 0, asyncReady, 0);
        await tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, nint zero)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            AsyncReady.ProgressCallbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            var res = CopyFinish(this, result, ref error);
            if (res)
                tcs.TrySetResult();
            else
            {
                var gerror = GError.Get(error, true);
                if (createTargetPath && gerror?.Domain == Quarks.Gio && gerror.Code == (int)IO.NotFound && Exists)
                {
                    var fi = new FileInfo(destination);
                    var destPath = fi.Directory;
                    try
                    {
                        destPath?.Create();
                    }
                    catch (UnauthorizedAccessException)
                    {
                        tcs.TrySetException(new GioException(Quarks.Gio, IO.PermissionDenied, "Access Denied"));
                    }
                    catch
                    {
                        tcs.TrySetException(new Exception("General Exception"));
                    }

                    await CopyAsync(destination, flags, true, cb, cancellation);
                }
                else
                {
                    if (gerror?.Domain == Quarks.Gio && gerror.Code == (int)IO.Cancelled)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetException(gerror?.Domain == Quarks.Gio ? new GioException(gerror, this) : GtkException.Get(gerror));
                }
            }
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_new_for_path", CallingConvention = CallingConvention.Cdecl)]
    extern static GFile _New(string path);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_load_contents", CallingConvention = CallingConvention.Cdecl)]
    extern static bool LoadContents(GFile gFile, nint cancellable, out nint content, out int length, nint etagOut, nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_async", CallingConvention = CallingConvention.Cdecl)]
    extern static void CopyAsync(GFile source, GFile destination, FileCopyFlags flags, int priority, nint cancellable,
        TwoLongAndPtrCallback? progress, nint _, ThreePointerDelegate asyncCallback, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_copy_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool CopyFinish(GFile source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_move_async", CallingConvention = CallingConvention.Cdecl)]
    extern static void MoveAsync(GFile source, GFile destination, FileCopyFlags flags, int priority, nint cancellable,
        TwoLongAndPtrCallback? progress, nint _, ThreePointerDelegate asyncCallback, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_move_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool MoveFinish(GFile source, nint asyncResult, ref nint error);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_trash_async", CallingConvention = CallingConvention.Cdecl)]
    extern static bool Trash(GFile file, int prio, nint cancellable, ThreePointerDelegate asyncCallback, nint _);

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

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_find_enclosing_mount", CallingConvention = CallingConvention.Cdecl)]
    extern static Mount FindEnclosingMount(GFile file, nint _, nint __);

    [DllImport(Libs.LibGtk, EntryPoint = "g_file_query_exists", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _Exists(GFile file, nint _);
    
}
