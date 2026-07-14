using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        mediaFile = MediaFile.New("/mnt/Home/uwe/Büro.mkv");
        var asp = (mediaFile as IPaintable).IntrinsicAspectRatio;
        var w = (mediaFile as IPaintable).IntrinsicWidth;
        var h = (mediaFile as IPaintable).IntrinsicHeight;
        mediaControls.SetMediaStream(mediaFile);
        video.SetPaintable(mediaFile);
        OnFinalize(mediaFile.Dispose);
    }

    [Widget]
    readonly Picture video = null!;

    [Widget]
    readonly MediaControls mediaControls = null!;

    readonly MediaFile mediaFile;
}

