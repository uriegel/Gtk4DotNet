using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Mount : GObject
{
    public string? Name { get => GetName(this).PtrToString(true); }
    public string? Uuid { get => GetUuid(this).PtrToString(true); }
    public GFile GetRoot()
    {
        var res = GetRoot(this);
        // Mount a living object:
        // res.CheckDiagnostics();
        return res;
    }

    public Volume GetVolume()
    {
        var volume = GetVolume(this);
        // Do not call
        //volume.CheckDiagnostics();      
        return volume;
    }

    public Task UnmountAsync(bool force = false)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        using var mo = MountOperation.New();
        Unmount(this, force ? UnmountFlags.Force: UnmountFlags.None, mo, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, nint __)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            if (UnmountFinish(this, result, ref error))
                tcs.TrySetResult();
            else
                tcs.TrySetException(GtkException.Get(error, true));
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_volume", CallingConvention = CallingConvention.Cdecl)]
    extern static Volume GetVolume(Mount mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_root", CallingConvention = CallingConvention.Cdecl)]
    public extern static GFile GetRoot(Mount mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetName(Mount mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_uuid", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetUuid(Mount mount);

    [DllImport(Libs.LibGio, EntryPoint = "g_mount_unmount_with_operation", CallingConvention = CallingConvention.Cdecl)]
    extern static void Unmount(Mount mount, UnmountFlags flags, MountOperation mountOperation, nint _, ThreePointerDelegate cb, nint __);

    [DllImport(Libs.LibGio, EntryPoint = "g_mount_unmount_with_operation_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool UnmountFinish(Mount mount, nint result, ref nint error);
}
