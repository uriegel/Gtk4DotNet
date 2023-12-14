using GtkDotNet;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

using static System.Console;
using CsTools.Functional;

static class BuilderProgram
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => app
            .SideEffect(app =>
                Builder.FromResource("/org/gtk/example/builder.ui").Use(
                    builder => builder
                        .GetObject<WindowHandle>("window", w => w
                            .SetApplication(app)
                            .SideEffect(w => 
                                builder
                                .SideEffect(b => b.GetObject<ButtonHandle>("button1", b => b
                                    .OnClicked(() => WriteLine("Button1 clicked"))))
                                .SideEffect(b => b.GetObject<ButtonHandle>("button2", b => b
                                    .OnClicked(() => WriteLine("Button2 clicked"))))
                                .SideEffect(b => b.GetObject<ButtonHandle>("quit", b => b
                                    .OnClicked(() => w.CloseWindow()))))
                            .Show()))))
            .Run(0, IntPtr.Zero);
}


