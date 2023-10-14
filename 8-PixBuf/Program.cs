using GtkDotNet;

using LinqTools;

void SaveThumbnail(IntPtr _)
    {
    var pb = Pixbuf.NewFromFile(@"/home/uwe/20230908_095205.jpg");
    Pixbuf.GetFileInfo(@"/home/uwe/20230908_095205.jpg", out var w, out var h);
    var newh = 64 * h / w;
    var thumbnail = Pixbuf.Scale(pb, 64, newh, Interpolation.Bilinear);
    GObject.Unref(pb);
    Pixbuf.SaveJpg(thumbnail, @"/home/uwe/thumbnail.jpg");
    GObject.Unref(thumbnail);
}

return Application.Run("org.gtk.example", app => 
    Application
        .NewWindow(app)
        .SideEffect(w => w.SetTitle("Hello Gtk PixBuf👍"))
        .SideEffect(w => w.SetDefaultSize(1200, 1200))
        .SideEffect(SaveThumbnail)
        .Show());





