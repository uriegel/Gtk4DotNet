using System.Runtime.InteropServices;
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

    public static void Eject(this VolumeHandle volume, UnmountFlags flags, MountOperationHandle mountOperation)
    {
        Eject(volume, flags, mountOperation, 0, (a, res, c) =>
        { 
            if (!EjectFinish(volume, res, out nint error))
            {
                var message = error.GetMessage();
                Console.WriteLine("Mount failed: " + message);
                //GLib.g_error_free(error);
            }
            else
            {
                Console.WriteLine("Mount successful!");
            }            
        }, 0);
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_eject_with_operation_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool EjectFinish(this VolumeHandle volume, nint result, out nint error);
    
    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(this VolumeHandle volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_identifier", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetIdentifier(this VolumeHandle volume, string kind);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_eject_with_operation", CallingConvention = CallingConvention.Cdecl)]
    extern static void Eject(this VolumeHandle volume, UnmountFlags flags, MountOperationHandle mountOperation, nint _, GAsyncReadyCallback cb, nint __);
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void GAsyncReadyCallback(nint sourceObject, nint res, nint userData);
