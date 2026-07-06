using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        video.SetFileName("/home/uwe/Downloads/Er fährt mit dem Fahrrad durch Afrikas gefährlichsten Dschungel (Hans Maggi) - Tim Gabel (1080p).mp4");
        video.AutoPlay = true;
    }

    [Widget]
    readonly Video video = null!;
}

