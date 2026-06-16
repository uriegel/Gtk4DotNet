using System.Runtime.InteropServices;
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
        => volumeRemovedId = SignalConnect<ThreePointerDelegate>("Volume-removed", (_, _, _) => onChanged(), true);

    [DllImport(Libs.LibGtk, EntryPoint = "g_volume_monitor_get", CallingConvention = CallingConvention.Cdecl)]
    extern static VolumeMonitor _Get();

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

