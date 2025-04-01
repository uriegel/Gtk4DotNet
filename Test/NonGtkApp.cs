using CsTools.Extensions;
using CsTools.Functional;
using GtkDotNet;
using GtkDotNet.Exceptions;
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

        await Gtk.Dispatch(() => WriteLine(ContentType.Guess(".pdf")));

        var thumbnail = SaveThumbnail("./resources/image.jpg");
        {
            using var file = GFile.New(thumbnail);
            await file.TrashAsync();
        }
        {
            using var file = GFile.New("non existent file");
            try
            {
                await file.TrashAsync();
            }
            catch (GFileException e)
            {
                WriteLine($"Error: {e}");
            }
        }

        const string testDirectory = "TestDirectory";
        Directory.CreateDirectory(testDirectory);

        // CopyFile(testDirectory.AppendPath("NonExisting.txt"), "non");
        // CopyFile(testDirectory.AppendPath("../First.cs"), "non/u");
        // CopyFile(testDirectory.AppendPath("../First.cs"), "/etc");
        // CopyFile(testDirectory.AppendPath("../First.cs"), "/etc/non");
        // CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"));
        // CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"));
        // CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"), FileCopyFlags.Overwrite);
        WriteLine();
        // CopyFile(testDirectory.AppendPath("../bin/Debug/net6.0/System.Linq.Async.dll"), testDirectory.AppendPath("linqasync.dll"), 
        //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
        WriteLine();

        // await CopyFileAsync(testDirectory.AppendPath("../bin/Debug/net6.0/System.Linq.Async.dll"), testDirectory.AppendPath("linqasync.dll"), 
        //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
        // WriteLine();
        // ReadLine();

        WriteLine();
        // CopyFile(testDirectory.AppendPath("/speicher/Videos/Burning.mp4"), testDirectory.AppendPath("burning.mp4"), 
        //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
        // WriteLine();

        // Directory.Delete(testDirectory, true);

        // Directory.CreateDirectory(testDirectory);

        // WriteLine("Cancel after 1s");
        // CopyFile(testDirectory.AppendPath("/speicher/Videos/Burning.mp4"), testDirectory.AppendPath("burning.mp4"), 
        //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"), token: new CancellationTokenSource(1000).Token);
        WriteLine();

        Directory.Delete(testDirectory, true);
    }

    static string SaveThumbnail(string file)
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
        return thumbnailFile.Name;
    }
}
