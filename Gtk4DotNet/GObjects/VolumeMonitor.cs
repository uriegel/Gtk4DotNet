using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class VolumeMonitor : GObject
{
    public static VolumeMonitor Get()
    {
        var vm = _Get();
        // Shared object
        //vm.CheckDiagnostics();
        return vm;
    }

    public DisposableEnumerable<Volume> GetVolumes()
    {
        var volumes = GetVolumes(this);
        nint current = volumes;
        var list = new List<Volume>();
        while (current != 0)
        {
            var glist = Marshal.PtrToStructure<GList>(current);
            var volume = new Volume();
            list.Add(volume);
            volume.SetInternalHandle(glist.Data);
            // volume.CheckDiagnostics();
            current = glist.Next;
        }
        GList.Free(volumes);
        return list.AsDisposable();
    }

    public DisposableEnumerable<Drive> GetConnectedDrives()
    {
        var drives = _GetDrives(this);
        nint current = drives;
        var list = new List<Drive>();
        while (current != 0)
        {
            var glist = Marshal.PtrToStructure<GList>(current);
            var drive = new Drive();
            list.Add(drive);
            drive.SetInternalHandle(glist.Data);
            drive.CheckDiagnostics();
            current = glist.Next;
        }
        GList.Free(drives);
        return list.AsDisposable();
    }

    public void OnDriveChanged(Action onChanged)
        => driveChangeId = SignalConnect<ThreePointerDelegate>("drive-changed", (_, _, _) => onChanged(), true);
    public void OnDriveConnected(Action onChanged)
        => driveConnectedId = SignalConnect<ThreePointerDelegate>("drive-connected", (_, _, _) => onChanged(), true);
    public void OnDriveDisconnected(Action onChanged)
        => driveDisconnectedId = SignalConnect<ThreePointerDelegate>("drive-disconnected", (_, _, _) => onChanged(), true);
    public void OnDriveEjectButton(Action onChanged)
        => driveEjectButtonId = SignalConnect<ThreePointerDelegate>("drive-eject-button", (_, _, _) => onChanged(), true);
    public void OnDriveStopButton(Action onChanged)
        => driveStopButtonId = SignalConnect<ThreePointerDelegate>("drive-stop-button", (_, _, _) => onChanged(), true);
    public void OnMountAdded(Action onChanged)
        => mountAddedId = SignalConnect<ThreePointerDelegate>("mount-added", (_, _, _) => onChanged(), true);
    public void OnMountChanged(Action onChanged)
        => mountChangedId = SignalConnect<ThreePointerDelegate>("mount-changed", (_, _, _) => onChanged(), true);
    public void OnMountPreUnmount(Action onChanged)
        => mountPreUnmountId = SignalConnect<ThreePointerDelegate>("mount-pre-unmount", (_, _, _) => onChanged(), true);
    public void OnMountRemoved(Action onChanged)
        => mountRemovedId = SignalConnect<ThreePointerDelegate>("mount-removed", (_, _, _) => onChanged(), true);
    public void OnVolumeAdded(Action onChanged)
        => volumeAddedId = SignalConnect<ThreePointerDelegate>("volume-added", (_, _, _) => onChanged(), true);
    public void OnVolumeChanged(Action onChanged)
        => volumeChangedId = SignalConnect<ThreePointerDelegate>("volume-changed", (_, _, _) => onChanged(), true);
    public void OnVolumeRemoved(Action onChanged)
        => volumeRemovedId = SignalConnect<ThreePointerDelegate>("volume-removed", (_, _, _) => onChanged(), true);

    [DllImport(Libs.LibGtk, EntryPoint = "g_volume_monitor_get", CallingConvention = CallingConvention.Cdecl)]
    extern static VolumeMonitor _Get();

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_monitor_get_volumes", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetVolumes(VolumeMonitor volumeMonitor);

    [DllImport(Libs.LibGio, EntryPoint = "g_volume_monitor_get_connected_drives", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetDrives(VolumeMonitor volumeMonitor);
    
    DelegateId? driveChangeId = null;
    DelegateId? driveConnectedId = null;
    DelegateId? driveDisconnectedId = null;
    DelegateId? driveEjectButtonId = null;
    DelegateId? driveStopButtonId = null;
    DelegateId? mountAddedId = null;
    DelegateId? mountChangedId = null;
    DelegateId? mountPreUnmountId = null;
    DelegateId? mountRemovedId = null;
    DelegateId? volumeAddedId = null;
    DelegateId? volumeChangedId = null;
    DelegateId? volumeRemovedId = null;

    #region IDisposable

    protected override void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            // if (disposing)
            //     ;
            // Verwalteten Zustand (verwaltete Objekte) bereinigen
            if (driveChangeId.HasValue)
                SignalDisconnect(driveChangeId.Value);
            if (driveConnectedId.HasValue)
                SignalDisconnect(driveConnectedId.Value);
            if (driveDisconnectedId.HasValue)
                SignalDisconnect(driveDisconnectedId.Value);
            if (driveEjectButtonId.HasValue)
                SignalDisconnect(driveEjectButtonId.Value);
            if (driveStopButtonId.HasValue)
                SignalDisconnect(driveStopButtonId.Value);
            if (mountAddedId.HasValue)
                SignalDisconnect(mountAddedId.Value);
            if (mountChangedId.HasValue)
                SignalDisconnect(mountChangedId.Value);
            if (mountPreUnmountId.HasValue)
                SignalDisconnect(mountPreUnmountId.Value);
            if (mountRemovedId.HasValue)
                SignalDisconnect(mountRemovedId.Value);
            if (volumeAddedId.HasValue)
                SignalDisconnect(volumeAddedId.Value);
            if (volumeChangedId.HasValue)
                SignalDisconnect(volumeChangedId.Value);
            if (volumeRemovedId.HasValue)
                SignalDisconnect(volumeRemovedId.Value);

            // Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // Große Felder auf NULL setzen

            disposedValue = true;
        }
        base.Dispose(disposing);
    }
    
    bool disposedValue;

    #endregion
}

