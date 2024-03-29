using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;

using static System.Console;

static class NonGtkApp
{
    public static int Run()
        => 0.SideEffect(_ => StartRun())
            .SideEffect(_ => ReadLine())
            .SideEffect(_ => Gtk.Stop());

    async static void StartRun()
    {
        Gtk.Start();

        await Gtk.Dispatch(() =>
        {
            GtkSettings
                .GetDefault()
                .SideEffect(s => WriteLine(s.GetString("gtk-theme-name")))
                .OnNotify("gtk-theme-name", s => WriteLine($" changed: {s.GetString("gtk-theme-name")}"));
        });

        await Gtk.Dispatch(() => WriteLine(Gtk.GuessContentType(".pdf")));

        SaveThumbnail("/daten/Bilder/Fotos/1995/1/Bild017.jpg");

        const string testDirectory = "TestDirectory";
        Directory.CreateDirectory(testDirectory);

        CopyFile(testDirectory.AppendPath("NonExisting.txt"), "non");
        CopyFile(testDirectory.AppendPath("../First.cs"), "non/u");
        CopyFile(testDirectory.AppendPath("../First.cs"), "/etc");
        CopyFile(testDirectory.AppendPath("../First.cs"), "/etc/non");
        CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"));
        CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"));
        CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"), FileCopyFlags.Overwrite);
        WriteLine();
        CopyFile(testDirectory.AppendPath("../bin/Debug/net6.0/System.Linq.Async.dll"), testDirectory.AppendPath("linqasync.dll"), 
            progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
        WriteLine();

        // await CopyFileAsync(testDirectory.AppendPath("../bin/Debug/net6.0/System.Linq.Async.dll"), testDirectory.AppendPath("linqasync.dll"), 
        //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
        // WriteLine();
        // ReadLine();

        WriteLine();
        CopyFile(testDirectory.AppendPath("/speicher/Videos/Burning.mp4"), testDirectory.AppendPath("burning.mp4"), 
            progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
        WriteLine();

        Directory.Delete(testDirectory, true);

        Directory.CreateDirectory(testDirectory);

        WriteLine("Cancel after 1s");
        CopyFile(testDirectory.AppendPath("/speicher/Videos/Burning.mp4"), testDirectory.AppendPath("burning.mp4"), 
            progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"), token: new CancellationTokenSource(1000).Token);
        WriteLine();

        Directory.Delete(testDirectory, true);


        void CopyFile(string source, string target, FileCopyFlags fileCopyFlags = FileCopyFlags.None, ProgressCallback? progress = null, CancellationToken? token = null)
            => GFile
                .New(source)
                .Use(f => f.Copy(target, fileCopyFlags, false, progress, token))
                .Match(
                    _ => WriteLine("File copied"),
                    e => WriteLine($"File not copied, {e.GetType()} {e.Code}, {(e is FileError fe ? fe.Error : "General error")}"));

        // Task CopyFileAsync(string source, string target, FileCopyFlags fileCopyFlags = FileCopyFlags.None, ProgressCallback? progress = null)
        //     => GFile
        //         .New(source)
        //         .UseAsync(f => f.CopyAsync(target, fileCopyFlags, progress))
        //         .MatchAsync(
        //             _ => 1.SideEffect(_ => WriteLine("File copied")),
        //             e => 1.SideEffect(_ => WriteLine($"File not copied, {e.GetType()} {e.Code}, {e}")));
    }

    static void SaveThumbnail(string file)
    {
        string GetThumbnailFilename(string file)
            => file += ".thumbnail.jpg";

        var pb = Pixbuf.NewFromFile(file);
        var (w, h) = file.GetFileInfo();
        var newh = 64 * h / w;
        var thumbnail = pb.Scale(64, newh, Interpolation.Bilinear);
        var stream = Pixbuf.SaveJpgToBuffer(thumbnail);
        using var thumbnailFile = File.Create(GetThumbnailFilename(file));
        stream?.CopyTo(thumbnailFile);
    }
}
