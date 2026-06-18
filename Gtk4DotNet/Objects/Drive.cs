using System.Runtime.InteropServices;
using Gtk4DotNet.Exceptions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Drive : GObject
{
    public bool CanEject { get => _CanEject(this); }
    public bool CanStart { get => _CanStart(this); }
    public bool CanStop { get => _CanStop(this); }
    public bool IsRemovable { get => _IsRemovable(this); }
    public string? Name { get => _GetName(this).PtrToString(true); }
    public string? UnixDevice { get => _GetIdentifier(this, "unix-device").PtrToString(true); }

    public Task StopAsync(bool force = false)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var mo = MountOperation.New();
        mo.OnAskQuestion(() => Console.WriteLine("Question from stop drive"));
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        Stop(this, force ? UnmountFlags.Force : UnmountFlags.None, mo, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, nint __)
        {
            mo.Dispose();
            AsyncReady.Callbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            if (!StopFinish(this, result, ref error))
            {
                var gerror = new GErrorStruct(error);

                // TODO
                var message = gerror.Message;
                Console.WriteLine("Eject failed: " + message);
                tcs.TrySetException(new VolumeException(message, Name, UnixDevice, gerror));
            }
            else
                tcs.TrySetResult();
        }
    }

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(Drive drive);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_get_identifier", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetIdentifier(Drive drive, string kind);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_can_start", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _CanStart(Drive drive);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_can_stop", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _CanStop(Drive drive);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_can_eject", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _CanEject(Drive drive);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_is_removable", CallingConvention = CallingConvention.Cdecl)]
    extern static bool _IsRemovable(Drive drive);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_stop", CallingConvention = CallingConvention.Cdecl)]
    extern static void Stop(Drive drive, UnmountFlags flags, MountOperation mo, nint _, ThreePointerDelegate cb, nint __);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_stop_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool StopFinish(Drive drive, nint result, ref nint error);
}
