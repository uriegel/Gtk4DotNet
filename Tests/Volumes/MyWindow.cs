using CsTools.Functional;
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

        settings = GSettings.New("org.gnome.desktop.interface");
        settings.OnChanged("gtk-theme", () => WriteLine($"Thema, {settings.GetString("gtk-theme")}"));

        monitor = VolumeMonitor.Get();
        using var volumes = monitor.GetVolumes();
        volumeNames = [.. volumes.Select(n => n.Name)];
        foreach (var volume in volumes)
            WriteLine($"Volume: {volume.Name}, {volume.UnixDevice}, {volume.Uuid}");

        monitor.OnDriveChanged(() => WriteLine("Drive changed"));
        monitor.OnDriveConnected(DriveConnected);
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

        async void DriveConnected()
        {
            try
            {
                using var volumes = monitor.GetVolumes();
                using var newVolume = volumes.FirstOrDefault(n => !volumeNames.Contains(n.Name));
                if (newVolume != null)
                    WriteLine($"Drive connected: {newVolume.Name}, {newVolume.UnixDevice}, {newVolume.Uuid}");

                await Task.Delay(10_000);

                WriteLine("Ejecting newly connected drive");
                if (newVolume != null)
                {
                    using var mount = newVolume.GetMount();
                    if (mount != null)
                        await mount.UnmountAsync(true);
                    await newVolume.EjectAsync(true);
                }
                    
            }
            catch (Exception e)
            {
                Error.WriteLine($"Could not eject: {e}");
            }
        }

        OnFinalize(() =>
        {
            monitor.Dispose();
            settings.Dispose();
        });
    }

    VolumeMonitor monitor;
    GSettings settings;
    string?[] volumeNames = [];
}