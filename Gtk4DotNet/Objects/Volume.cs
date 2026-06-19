using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Volume : GObject
{ 
    public string? Name { get => _GetName(this).PtrToString(true); }

    public string? UnixDevice { get => _GetIdentifier(this, "unix-device").PtrToString(true); }

    public string? Uuid { get => _GetUuid(this).PtrToString(true); }
    
    public bool CanMount { get => _CanMount(this); }

    public bool CanEject { get => _CanEject(this); }

    public Mount? GetMount()
    {
        var mount = GetMount(this);
        if (mount.IsInvalid)
            return null;
        // DO not
        //mount.CheckDiagnostics();
        return mount;
    }

    public Drive? GetDrive()
    {
        var drive = GetDrive(this);
        if (drive.IsInvalid)
            return null;
        // Do not call this
        // drive.CheckDiagnostics();
        return drive;
    }

    public Task EjectAsync(bool force = false)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var mo = MountOperation.New();
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        Eject(this, force ? UnmountFlags.Force : UnmountFlags.None, mo, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, nint __)
        {
            mo.Dispose();
            AsyncReady.Callbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            if (EjectFinish(this, result, ref error))
                tcs.TrySetResult();
            else
                tcs.TrySetException(GtkException.Get(error, true));
        }
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_can_mount", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _CanMount(Volume volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_can_eject", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _CanEject(Volume volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(Volume volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_identifier", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetIdentifier(Volume volume, string kind);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_uuid", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetUuid(Volume volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_mount", CallingConvention = CallingConvention.Cdecl)]
    extern static Mount GetMount(Volume volume);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_drive", CallingConvention = CallingConvention.Cdecl)]
    extern static Drive GetDrive(Volume volume);
    
    [DllImport(Libs.LibGio, EntryPoint = "g_volume_eject_with_operation", CallingConvention = CallingConvention.Cdecl)]
    extern static void Eject(Volume volume, UnmountFlags flags, MountOperation mountOperation, nint _, ThreePointerDelegate cb, nint __);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_eject_with_operation_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool EjectFinish(Volume volume, nint result, ref nint error);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_get_icon", CallingConvention = CallingConvention.Cdecl)]
    extern static GIcon _GetIcon(Volume volume);
}


// TODO

    // public static string? GetIcon(this VolumeHandle volume)
    // {
    //     using var i = _GetIcon(volume);
    //     var strings = Icon.Names(i);
    //     return strings[2];
    // }



