using CsTools.Extensions;
using Gtk4DotNet;

Application
    .New("de.uriegel.gtk4dotnet")
    .WithDiagnostics(true)
    .OnActivate(app => app
        .NewWindow()
        .SideEffect(a => VolumeMonitoring())
        .Title("Volume Monitoring👍")
        .DefaultSize(600, 200)
        .Show()
    ).Run();

void VolumeMonitoring()
{
    using var probeFile = GFile.New("/media/uwe/Daten/Bilder/Fotos/1965/Bild001.jpg");
    using var mount = probeFile.FindEnclosingMount();
    using var root = mount?.GetRoot();
}

// TODO is _floating
// TODO Chech refCount

