using System.Reflection;
using System.Runtime.InteropServices;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        //var video = Picture.New();
        //video.KeepAspectRatio = false;
        //            <property name="halign">fill</property>
        //            <property name="valign">fill</property>
        mediaFile = MediaFile.New("/mnt/Home/uwe/Büro.mkv");
        var asp = (mediaFile as IPaintable).IntrinsicAspectRatio;
        var w = (mediaFile as IPaintable).IntrinsicWidth;
        var h = (mediaFile as IPaintable).IntrinsicHeight;
        mediaControls.SetMediaStream(mediaFile);
        video.SetPaintable(mediaFile);
        var test = video.AutoDestroyed;
        test = video.AutoDestroyed;
        test = video.AutoDestroyed;
        test = video.AutoDestroyed;
        test = video.AutoDestroyed;
        var wn = video.WidgetName;
        test = video.AutoDestroyed;
        test = video.AutoDestroyed;
        wn = video.WidgetName;
                

        //var test = tgtk_aspect_container_new();
        //overlay.SetChild(test);
        //SetValue(test, "aspect-ratio", 16.0 / 9, 0);


        // video.SetSizeRequest(160, 90);
        // video.MarginStart = 50;
        OnFinalize(mediaFile.Dispose);
        //tgtk_aspect_container_set_child(videoContainer.GetInternalHandle(), video.GetInternalHandle());



        // später();
        // async void später()
        // {
        //     await Task.Delay(500);
        //     var asp = (mediaFile as IPaintable).IntrinsicAspectRatio;
        //     var w = (mediaFile as IPaintable).IntrinsicWidth;
        //     var h = (mediaFile as IPaintable).IntrinsicHeight;
        // }
    }

    [Widget]
    readonly Picture video = null!;

    [Widget]
    readonly Overlay overlay = null!;

    [Widget]
    readonly Widget videoContainer = null!;

    [Widget]
    readonly MediaControls mediaControls = null!;

    readonly MediaFile mediaFile;


    [DllImport("libtgtk4dotnet.so", CallingConvention = CallingConvention.Cdecl)]
    static extern nint tgtk_aspect_container_new();

    [DllImport("libtgtk4dotnet.so", CallingConvention = CallingConvention.Cdecl)]
    static extern void tgtk_aspect_container_set_child(nint ac, nint widget);

    [DllImport("libgtk-4.so.1", EntryPoint = "g_object_set", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetValue(nint obj, string name, double value, nint end);

    
}

