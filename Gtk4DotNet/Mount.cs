using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Mount
{
    public static Task Remount(this MountHandle mount)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        Remount(mount, 0, 0, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(IntPtr _, IntPtr result, IntPtr zero)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            tcs.TrySetResult();
        }
    }

    public static string GetName(this MountHandle mount)
        => _GetName(mount).PtrToString(true) ?? "";

    public static string GetUuid(this MountHandle mount)
        => _GetUuid(mount).PtrToString(true) ?? "";

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_volume", CallingConvention = CallingConvention.Cdecl)]
    public extern static VolumeHandle GetVolume(this MountHandle mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(MountHandle mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_uuid", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetUuid(MountHandle mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_remount", CallingConvention = CallingConvention.Cdecl)]
    extern static void Remount(MountHandle mount, int _, nint __, nint ___, ThreePointerDelegate asyncCallback, nint ____);
}
