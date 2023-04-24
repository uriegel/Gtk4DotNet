using GtkDotNet;
using LinqTools;

using static System.Console;

Application.Start();

WriteLine($"GTK initialized, press any key to stop...");


// Retrieve icon file from mime type
await Application.Dispatch(() => GObjectRef
    .WithRef(GtkSettings.GetDefault())
    .Use(Settings =>
        Settings
            .Value
            .SideEffect(s => WriteLine(s.GetString("gtk-theme-name")))
            .SignalConnect("notify::gtk-theme-name", () => WriteLine("Theme Changed"))));

await Application
        .Dispatch(() => WriteLine(Gtk.GuessContentType(".pdf")));

try 
{
    await Application.Dispatch(() =>
    {
        var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "test.tst");
        var f = File.Create(filename);
        f.Close();
        GFile.Trash(filename);

        GFile.Trash("/notfound/nofile.txt");
        GFile.Trash("/etc/fstab");
    });
}
catch (Exception e)
{
    Error.WriteLine($"Could not delete: {e}");
}

ReadLine();
Application.Stop();
WriteLine("GTK uninitialized, terminated");


