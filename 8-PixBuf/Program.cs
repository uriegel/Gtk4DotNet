using GtkDotNet;

using LinqTools;

void SaveThumbnails(IntPtr _)
{
    Directory
        .EnumerateFiles(@"/home/uwe/Canon")
        .ForEach(SaveThumbnail);

    void SaveThumbnail(string file)
    {
        var pb = Pixbuf.NewFromFile(file);
        Pixbuf.GetFileInfo(file, out var w, out var h);
        var newh = 64 * h / w;
        var thumbnail = Pixbuf.Scale(pb, 64, newh, Interpolation.Bilinear);
        GObject.Unref(pb);
        var stream = Pixbuf.SaveJpgToBuffer(thumbnail);
        GObject.Unref(thumbnail);
        using var thumbnailFile = File.Create(GetThumbnailFilename(file));
        stream?.CopyTo(thumbnailFile);
    }

    string GetThumbnailFilename(string file)
        => file += ".thumbnail.jpg";
}

SaveThumbnails(IntPtr.Zero);

return Application.Run("org.gtk.example", app => 
    Application
        .NewWindow(app)
        .SideEffect(w => w.SetTitle("Hello Gtk PixBuf👍"))
        .SideEffect(w => w.SetDefaultSize(1200, 1200))
        .SideEffect(SaveThumbnails)
        .Show());





