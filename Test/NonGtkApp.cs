using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.Extensions;
using static System.Console;

[StructLayout(LayoutKind.Sequential)]
struct GList {
    public nint data;
    public  nint next;
    public nint prev;

    [DllImport("libgtk-4.so.1", EntryPoint = "g_list_free ", CallingConvention = CallingConvention.Cdecl)]
    internal extern static IntPtr Free(IntPtr list);
}
static class NonGtkApp
{
    public static int Run()
    {
        Gtk.Start();

        var vm = VolumeMonitor.Get();
        var volumes = vm.GetVolumes();
        foreach (var volume in volumes)
            WriteLine($"Volume: {volume.GetName()}, {volume.CanMount()}, {volume.CanEject()}, {volume.GetUnixDevice()}");

        var sde1 = volumes.FirstOrDefault(n => n.GetUnixDevice() == "/dev/sde1");
        if (sde1 != null)
        {
            using var mo = MountOperation.New();
            sde1.Eject(UnmountFlags.Force, mo);
        }

        //GVolumeMonitor *monitor = g_volume_monitor_get();
            //     var monitor = g_volume_monitor_get();
            // var volumes = g_volume_monitor_get_volumes(monitor);
            // var l = volumes;
            // while (true)
            // {
            //     var glist = Marshal.PtrToStructure<GList>(l);
            //     if (g_volume_can_mount(glist.data))
            //     {
            //         WriteLine($"Found mountable volume: {g_volume_get_name(glist.data).PtrToString(false)}");
            //         var mo = g_mount_operation_new();
            //         g_volume_mount(glist.data, 0, mo, 0, (a, b, c) =>
            //         {

            //         }, 0);
            //     }
            // }



            RunOnUIThread();

        var tempDir = Path.GetTempPath().AppendPath("GtkDotNet");
        var target = tempDir.AppendPath("testfile");


        ReadLine();
        Gtk.Stop();
        return 0;
    }

    [DllImport("libgtk-4.so.1", EntryPoint = "g_volume_monitor_get", CallingConvention = CallingConvention.Cdecl)]
    extern static nint g_volume_monitor_get();

    [DllImport("libgtk-4.so.1", EntryPoint = "g_volume_monitor_get_volumes", CallingConvention = CallingConvention.Cdecl)]
    extern static nint g_volume_monitor_get_volumes(nint monitor);

    [DllImport("libgtk-4.so.1", EntryPoint = "g_volume_can_mount", CallingConvention = CallingConvention.Cdecl)]
    extern static bool g_volume_can_mount(nint volume);

    [DllImport("libgtk-4.so.1", EntryPoint = "g_volume_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint g_volume_get_name(nint volume);

    [DllImport("libgtk-4.so.1", EntryPoint = "g_mount_operation_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint g_mount_operation_new();
    
    [DllImport("libgtk-4.so.1", EntryPoint = "g_volume_mount", CallingConvention = CallingConvention.Cdecl)]
    extern static void g_volume_mount(nint volume, int mmf, nint mo, nint cancel, GAsyncReadyCallback cb, nint nil);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GAsyncReadyCallback(nint sourceObject, nint res, nint userData);






    async static void RunOnUIThread()
    {
        await Gtk.Dispatch(() =>
        {
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
