using Gtk4DotNet;

using static System.Console;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        using var probeFile = GFile.New("/media/uwe/Daten/Bilder/Fotos/1965/Bild001.jpg");
        using var mount = probeFile.FindEnclosingMount();
        using var root = mount?.GetRoot();
        var refcount = root?.GetRefCount();
        monitor = VolumeMonitor.Get();
        monitor.OnDriveChanged(() => WriteLine("Drive changed"));
        monitor.OnDriveConnected(() => WriteLine("Drive connected"));
        monitor.OnDriveDisconnected(() => WriteLine("Drive disconnected"));
        monitor.OnDriveEjectButton(() => WriteLine("Drive eject button"));
        monitor.OnDriveStopButton(() => WriteLine("Drive stop button"));
        monitor.OnMountAdded(() => WriteLine("Mount added"));
        monitor.OnMountChanged(() => WriteLine("Mount changed"));
        monitor.OnMountPreUnmount(() => WriteLine("Mount pre unmount"));
        monitor.OnMountRemoved(() => WriteLine("Mount removed"));
        monitor.OnVolumeAdded(() => WriteLine("Volume added"));
        monitor.OnVolumeChanged(() => WriteLine("Volume changed"));
        monitor.OnVolumeRemoved(() => WriteLine("Volume removed"));

        OnFinalize(monitor.Dispose);
    }

    VolumeMonitor monitor;
}