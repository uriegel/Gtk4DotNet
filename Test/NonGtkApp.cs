using GtkDotNet;
using LinqTools;

using static System.Console;

static class NonGtkApp
{
    public static int Run()
    {
        Gtk.Start();

        Gtk.Dispatch(() =>
        {
            GtkSettings
                .GetDefault()
                .SideEffect(s => WriteLine(s.GetString("gtk-theme-name")))
                .OnNotify("gtk-theme-name", s => WriteLine($" changed: {s.GetString("gtk-theme-name")}"));
        });

        Gtk.Dispatch(() => WriteLine(Gtk.GuessContentType(".pdf")));

        ReadLine();

        Gtk.Stop();
        return 0;
    }
}
