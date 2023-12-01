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

        // TODO GFileError enum
        CopyFile(testDirectory.AppendPath("NonExisting.txt"), "non");
        CopyFile(testDirectory.AppendPath("../First.cs"), "non/u");
        CopyFile(testDirectory.AppendPath("../First.cs"), "/etc");
        CopyFile(testDirectory.AppendPath("../First.cs"), "/etc/non");

        Directory.Delete(testDirectory);

        void CopyFile(string source, string target)
            => GFile
                .New(source)
                .Use(f => f.Copy(target, FileCopyFlags.Overwrite, null))
                .Match(
                    _ => WriteLine("File copied"),
                    e => WriteLine($"File not copied, {e.Code}, {e}"));
    }
}
