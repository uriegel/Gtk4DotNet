using GtkDotNet;
using CsTools.Extensions;
using static System.Console;

static class PackingButtons
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                    .Title("Hello Gtk👍")
                    .SideEffect(win => win
                    .Child(
                        Grid
                            .New()
                            .Attach(                                
                                Button
                                    .NewWithLabel("Button 1")
                                    .OnClicked(() => WriteLine("Button1 clicked")), 0, 0, 1, 1)
                            .Attach(                                
                                Button
                                    .NewWithLabel("Button 2")
                                    .OnClicked(() => WriteLine("Button2 clicked")), 1, 0, 1, 1)
                            .Attach(                                
                                Button
                                    .NewWithLabel("Quit")
                                    .OnClicked(() => win.CloseWindow()), 0, 1, 2, 1)))
                    .Show())
            .Run(0, IntPtr.Zero);
}

