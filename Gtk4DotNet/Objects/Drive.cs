using System.Diagnostics;
using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;
using Gtk4DotNet.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class Drive : GObject
{
    public bool CanEject { get => _CanEject(this); }
    public bool CanStart { get => _CanStart(this); }
    public bool CanStop { get => _CanStop(this); }
    public bool IsRemovable { get => _IsRemovable(this); }
    public string Name { get => _GetName(this).PtrToString(true) ?? ""; }
    public string UnixDevice { get => _GetIdentifier(this, "unix-device").PtrToString(true) ?? ""; }

    public DisposableEnumerable<Volume> GetVolumes()
    {
        var volumes = _GetVolumes(this);
        nint current = volumes;
        var list = new List<Volume>();
        while (current != 0)
        {
            var glist = Marshal.PtrToStructure<GList>(current);
            var volume = new Volume();
            list.Add(volume);
            volume.SetInternalHandle(glist.Data);
            // TODO
            // volume.CheckDiagnostics();
            current = glist.Next;
        }
        GList.Free(volumes);
        return list.AsDisposable();
    }

    public async Task StopOrEjectAsync(Func<string?, string[], Process[], Task<bool>> showProcesses, Action<string?, string?, ulong, ulong> showProgress)
    {
        while (true)
        {
            TaskCompletionSource<bool>? tcs = null;
            Task? showProcessesTask = null;
            try
            {
                if (CanEject)
                    await EjectAsync(ShowProcesses, showProgress);
                else if (CanStop)
                    await StopAsync(ShowProcesses, showProgress);
                break;

                void ShowProcesses(string? msg, string[] choices, Process[] processes)
                {
                    tcs = new TaskCompletionSource<bool>();
                    showProcessesTask = Run();
                    async Task Run() => tcs.TrySetResult(await showProcesses(msg, choices, processes));
                }
            }
            catch (Exception)
            {
                if (tcs != null)
                {
                    var goOn = await tcs.Task;
                    if (!goOn)
                        throw;
                }
                else
                    throw;
            }
        }
    }
    public Task StopAsync(Action<string?, string[], Process[]>? showProcesses = null, Action<string?, string?, ulong, ulong>? onProgress = null, bool force = false)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var mo = MountOperation.New();
        mo.OnAskQuestion(() => Console.WriteLine("Question from mount operation not implemented"));
        if (showProcesses != null)
            mo.OnShowProcesses(showProcesses);
        if (onProgress != null)
            mo.ShowUnmountProgress(onProgress);
        var asyncReady = new ThreePointerDelegate(AsyncReadyCallback);
        AsyncReady.Callbacks[id] = asyncReady;
        Stop(this, force ? UnmountFlags.Force : UnmountFlags.None, mo, 0, asyncReady, 0);
        return tcs.Task;

        async void AsyncReadyCallback(nint _, nint result, nint __)
        {
            mo.Dispose();
            AsyncReady.Callbacks.Remove(id, out var _);
            var error = IntPtr.Zero;
            if (StopFinish(this, result, ref error))
                tcs.TrySetResult();
            else
                tcs.TrySetException(GtkException.Get(error, true));
        }
    }

    public Task EjectAsync(Action<string?, string[], Process[]>? showProcesses = null, Action<string?, string?, ulong, ulong>? onProgress = null, bool force = false)
    {
        var tcs = new TaskCompletionSource();
        var id = AsyncReady.GetId();
        var mo = MountOperation.New();
        mo.OnAskQuestion(() => Console.WriteLine("Question from mount operation not implemented"));
        if (showProcesses != null)
            mo.OnShowProcesses(showProcesses);
        if (onProgress != null)
            mo.ShowUnmountProgress(onProgress);
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

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_eject_with_operation", CallingConvention = CallingConvention.Cdecl)]
    extern static void Eject(Drive drive, UnmountFlags flags, MountOperation mountOperation, nint _, ThreePointerDelegate cb, nint __);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_eject_with_operation_finish", CallingConvention = CallingConvention.Cdecl)]
    extern static bool EjectFinish(Drive drive, nint result, ref nint error);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_get_volumes", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetVolumes(Drive drive);
}
