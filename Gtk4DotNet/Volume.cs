using System.Runtime.InteropServices;
using GtkDotNet.Exceptions;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Volume
{
    [DllImport(Libs.LibGio, EntryPoint = "g_volume_can_mount", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool CanMount(this VolumeHandle volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_can_eject", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool CanEject(this VolumeHandle volume);

    public static string? GetName(this VolumeHandle volume)
        => volume._GetName().PtrToString(true);

    public static string? GetUnixDevice(this VolumeHandle volume)
        => volume._GetIdentifier("unix-device").PtrToString(true);

    public static string? GetUuid(this VolumeHandle volume)
        => _GetUuid(volume).PtrToString(true);

    public static Task EjectAsync(this VolumeHandle volume, UnmountFlags flags, MountOperationHandle mountOperation)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        Eject(volume, flags, mountOperation, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, nint __)
        {
            AsyncReady.Callbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            if (!EjectFinish(volume, result, ref error))
            {
                var gerror = new GErrorStruct(error);

                var message = gerror.Message;
                Console.WriteLine("Eject failed: " + message);
                tcs.TrySetException(new VolumeException(message, volume.GetName(), volume.GetUnixDevice(), gerror));
            }
            else
                tcs.TrySetResult();
        }
    }

    public static string? GetIcon(this VolumeHandle volume)
    {
        using var i = _GetIcon(volume);
        var strings = Icon.Names(i);
        return strings[2];
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_eject_with_operation_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool EjectFinish(this VolumeHandle volume, nint result, ref nint error);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(this VolumeHandle volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_identifier", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetIdentifier(this VolumeHandle volume, string kind);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_uuid", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetUuid(this VolumeHandle volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_eject_with_operation", CallingConvention = CallingConvention.Cdecl)]
    extern static void Eject(this VolumeHandle volume, UnmountFlags flags, MountOperationHandle mountOperation, nint _, ThreePointerDelegate cb, nint __);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_icon", CallingConvention = CallingConvention.Cdecl)]
    extern static IconHandle _GetIcon(VolumeHandle volume);
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void GAsyncReadyCallback(nint sourceObject, nint res, nint userData);

// TODO g_volume_get_icon
