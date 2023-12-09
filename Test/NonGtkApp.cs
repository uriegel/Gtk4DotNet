using CsTools.Extensions;
using GtkDotNet;
using LinqTools;

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

        void CopyFile(string source, string target, FileCopyFlags fileCopyFlags = FileCopyFlags.None, ProgressCallback? progress = null)
            => GFile
                .New(source)
                .Use(f => f.Copy(target, fileCopyFlags, progress))
                .Match(
                    _ => WriteLine("File copied"),
                    e => WriteLine($"File not copied, {e.GetType()} {e.Code}, {e}"));

        // Task CopyFileAsync(string source, string target, FileCopyFlags fileCopyFlags = FileCopyFlags.None, ProgressCallback? progress = null)
        //     => GFile
        //         .New(source)
        //         .UseAsync(f => f.CopyAsync(target, fileCopyFlags, progress))
        //         .MatchAsync(
        //             _ => 1.SideEffect(_ => WriteLine("File copied")),
        //             e => 1.SideEffect(_ => WriteLine($"File not copied, {e.GetType()} {e.Code}, {e}")));
    }
}
