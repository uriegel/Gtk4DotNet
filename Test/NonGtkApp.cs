using CsTools.Extensions;
using GtkDotNet;
using static System.Console;

static class NonGtkApp
{
    public static int Run()
    {
        Gtk.Start();
        RunOnUIThread();

        var tempDir = Path.GetTempPath().AppendPath("GtkDotNet");
        var target = tempDir.AppendPath("testfile");


        ReadLine();
        Gtk.Stop();
        return 0;
    }

    async static void RunOnUIThread()
    {
        await Gtk.Dispatch(async () =>
        {
            {
                using var file = GFile.New("/home/uwe/Dokumente/Urlaub/Klassentreffen 2025.odt");
                using var info = file.QueryContentType();
                var contentType = info.GetContentType();
                using var list = AppInfo.GetRecommendedApps(contentType ?? "");
                var items = list.Select(n => new { Name = n.GetName(), Executable = n.GetExecutable(), Icon = n.GetIcon() }).ToArray();
                using var list2 = AppInfo.GetAllApps();
                var items2 = list2.Select(n => new { Name = n.GetName(), Executable = n.GetExecutable(), Icon = n.GetIcon() }).ToArray();
            }

            var vm = VolumeMonitor.Get();
            var volumes = vm.GetVolumes();
            foreach (var volume in volumes)
                WriteLine($"Volume: {volume.GetName()}, {volume.CanMount()}, {volume.CanEject()}, {volume.GetUnixDevice()}");

            var sde1 = volumes.FirstOrDefault(n => n.GetUnixDevice() == "/dev/sde1");
            if (sde1 != null)
            {
                try
                {
                    using var mo = MountOperation.New();
                    await sde1.EjectAsync(UnmountFlags.Force, mo);
                }
                catch (Exception e)
                {
                    WriteLine($"{e}");
                }
            }

            WriteLine(ContentType.Guess(".pdf"));
            GtkSettings
                .GetDefault()
                .SideEffect(s => WriteLine(s.GetString("gtk-theme-name")))
                .OnNotify("gtk-theme-name", s => WriteLine($" changed: {s.GetString("gtk-theme-name")}"));
        });
    }

    // var thumbnail = SaveThumbnail("./resources/image.jpg");
    // {
    //     using var file = GFile.New(thumbnail);
    //     await file.TrashAsync();
    // }
    // {
    //     using var file = GFile.New("non existent file");
    //     try
    //     {
    //         await file.TrashAsync();
    //     }
    //     catch (GFileException e)
    //     {
    //         WriteLine($"Error: {e}");
    //     }
    // }

    // const string testDirectory = "TestDirectory";
    // Directory.CreateDirectory(testDirectory);

    // CopyFile(testDirectory.AppendPath("NonExisting.txt"), "non");
    // CopyFile(testDirectory.AppendPath("../First.cs"), "non/u");
    // CopyFile(testDirectory.AppendPath("../First.cs"), "/etc");
    // CopyFile(testDirectory.AppendPath("../First.cs"), "/etc/non");
    // CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"));
    // CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"));
    // CopyFile(testDirectory.AppendPath("../First.cs"), testDirectory.AppendPath("First.cs"), FileCopyFlags.Overwrite);
    //        WriteLine();
    // CopyFile(testDirectory.AppendPath("../bin/Debug/net6.0/System.Linq.Async.dll"), testDirectory.AppendPath("linqasync.dll"), 
    //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));

    // await CopyFileAsync(testDirectory.AppendPath("../bin/Debug/net6.0/System.Linq.Async.dll"), testDirectory.AppendPath("linqasync.dll"), 
    //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
    // WriteLine();
    // ReadLine();

    // CopyFile(testDirectory.AppendPath("/speicher/Videos/Burning.mp4"), testDirectory.AppendPath("burning.mp4"), 
    //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"));
    // WriteLine();

    // Directory.Delete(testDirectory, true);

    // Directory.CreateDirectory(testDirectory);

    // WriteLine("Cancel after 1s");
    // CopyFile(testDirectory.AppendPath("/speicher/Videos/Burning.mp4"), testDirectory.AppendPath("burning.mp4"), 
    //     progress: (c, t) => WriteLine($"Copy progress: {c}/{t}"), token: new CancellationTokenSource(1000).Token);
    //     WriteLine();

    //     Directory.Delete(testDirectory, true);
    // }

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
