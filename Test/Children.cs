using GtkDotNet;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;
using static System.Console;

static class Children
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                .NewWindow()
                .Name("Window")
                .Title("Hello Gtk👍")
                .SideEffect(win => win
                .Child(
                    Grid
                    .New()
                    .Name("Grid")
                    .Attach(                                
                        Button
                        .NewWithLabel("Button 1")
                        .Name("Btn1")
                        .OnClicked(() => WriteLine("Button1 clicked")), 0, 0, 1, 1)
                    .Attach(                                
                        Button
                        .NewWithLabel("Button 2")
                        .Name("Btn2")
                        .OnClicked(() => WriteLine("Button2 clicked")), 1, 0, 1, 1)
                    .Attach(             
                        Paned.New(Orientation.Horizontal)
                        .StartChild(Button
                            .NewWithLabel("Test")
                            .Name("Btn3")
                            .OnClicked(() => GetChildren(win)), true, true) 
                        .EndChild(Button
                            .NewWithLabel("Quit")   
                            .Name("Btn4")
                            .OnClicked(() => win.CloseWindow()), true, true), 
                        0, 1, 2, 1)))
                .Show())
            .Run(0, IntPtr.Zero);

    static void GetChildren(WindowHandle win)
    {
        win
            .GetAllChildren()
            .Select(n => n.GetName())
            .ForEach(WriteLine);
        WriteLine();
        WriteLine(win.FindWidget(w => w.GetName() == "Btn3")?.GetName());
        WriteLine();
        WriteLine(win.FindWidget(w => w.GetName() == "Not found")?.GetName() ?? "not found");
    }
}

