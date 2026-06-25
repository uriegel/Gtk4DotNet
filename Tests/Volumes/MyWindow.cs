using CsTools.Functional;
using Gtk4DotNet;

using static System.Console;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        //using var probeFile = GFile.New("/media/uwe/Daten/Bilder/Fotos/1965/Bild001.jpg");
        //using var probeFile = GFile.New("/media/uwe/Ubuntu 25.10 amd64");
        using var probeFile = GFile.New("/media/uwe/Videos/videos");
        using var mount = probeFile.FindEnclosingMount();
        using var vol = mount?.GetVolume();

        TestEjectOrStop();

        async void TestEjectOrStop()
        {
            try
            {
                using var driv = vol?.GetDrive();
                // using var mounts = driv?.GetVolumes().SelectFilterNull(n => n.GetMount()).AsDisposable();
                // if (mounts != null)
                //     foreach (var mount in mounts)
                //         await mount.UnmountAsync(OnShowProcesses);
                // static void OnShowProcesses(string? msg, string[] _, Process[] processes)
                //     => WriteLine($"{msg} {string.Join(" - ", processes.Select(n => n.ProcessName))}");
                if (driv != null)
                    await driv.StopOrEjectAsync(async (msg, _, processes) =>
                    {
                        var dialog = AdwAlertDialog.New("Cannot unmount", $"{msg}\n{string.Join("\n", processes.Select(n => n.ProcessName))}");
                        dialog.SetResponses([
                                new("retry", "Retry", Default: true, Appearance: AdwResponseAppearance.Suggested),
                                new("cancel", "_Cancel", Cancel: true)
                            ]);
                        var res = await dialog.PresentAsync(this);
                        return res == "retry";
                    }, (t, m, tl, bl) => WriteLine($"{t} - {m} - {tl} - {bl}"));
            }
            catch (Exception e)
            {
                Error.WriteLine(e);
            }
        }

        using var root = mount?.GetRoot();

        settings = GSettings.New("org.gnome.desktop.interface");
        settings["gtk-theme"].OnChanged += OnThemeChanged;

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
            // try
            // {
            //     using var volumes = monitor.GetVolumes();
            //     using var newVolume = volumes.FirstOrDefault(n => !volumeNames.Contains(n.Name));
            //     if (newVolume != null)
            //         WriteLine($"Drive connected: {newVolume.Name}, {newVolume.UnixDevice}, {newVolume.Uuid}");

            //     await Task.Delay(10_000);

            //     WriteLine("Ejecting newly connected drive");
            //     if (newVolume != null)
            //     {
            //         using var mount = newVolume.GetMount();
            //         if (mount != null)
            //             await mount.UnmountAsync(true);

            //         using var drive = newVolume.GetDrive();
            //         if (drive?.CanStop == true)
            //             await drive.StopAsync(true);

            //         if (newVolume.CanEject)
            //             await newVolume.EjectAsync(true);
            //     }

            // }
            // catch (Exception e)
            // {
            //     Error.WriteLine($"Could not eject: {e}");
            // }
        }

        OnFinalize(() =>
        {
            monitor.Dispose();
            settings["gtk-theme"].OnChanged -= OnThemeChanged;
            settings.Dispose();
        });
    }
    
    void OnThemeChanged() => WriteLine($"Thema, {settings.GetString("gtk-theme")}");

    VolumeMonitor monitor;
    GSettings settings;
    string?[] volumeNames = [];
}