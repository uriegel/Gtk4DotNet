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

        OnFinalize(monitor.Dispose);
    }

    VolumeMonitor monitor;
}