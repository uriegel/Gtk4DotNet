using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        var videoFile = "/mnt/Home/uwe/Büro.mkv";
        //var videoFile = "/run/media/uwe/Daten/Videos/Boing Boing.mp4";
        //var videoFile = "/run/media/uwe/Daten/Videos/essen.mkv";
        Streamer.Init();
        using var discoverer = new Discoverer(TimeSpan.FromSeconds(10));
        using var info = discoverer.DiscoverUri($"file://{videoFile}");
        using var videos = info!.GetVideoStreams();
        var videoInfo = videos.FirstOrDefault();
        var dar = videoInfo?.DisplayAspectRatio ?? 1;
        videoContainer.AspectRatio = dar;
        mediaFile = MediaFile.New(videoFile);
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

    [Widget]
    readonly AspectContainer videoContainer = null!;
    
    readonly MediaFile mediaFile;
}

