using GtkDotNet;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;
using GtkDotNet.SubClassing;

using static System.Console;

static class ProgressSubclass
{
    public static int Run()
        => Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .SubClass(new ProgressWindowClass(GTypeEnum.Window, "ProgressWindow", p => new ProgressWindow(p)))
                    .SideEffect(a =>
                        GObject.New<WindowHandle>("ProgressWindow".TypeFromName())
                        .SetApplication(app)
                        .Show()))                    
            .Run(0, IntPtr.Zero);



                    //         .PackEnd(
                    //             Revealer.New()
                    //             .SideEffect(r => progressStarter.Ref.BindProperty("active", r, "reveal-child", BindingFlags.Default))
                    //             .OnNotify("reveal-child", MakeProgress)
                    //             .TransitionType(RevealerTransition.SlideLeft)
                    //             .Child(
                    //                 MenuButton.New()
                    //                 .Popover(
                    //                     Popover.New()
                    //                     .Child(
                    //                         ProgressBar.New()
                    //                         .Ref(progressBar)
                    //                         .ShowText()
                    //                         .Fraction(.04)
                    //                     )
                    //                 )
                    //                 .Child(
                    //                     DrawingArea.New()
                    //                     .Ref(drawingArea)
                    //                     .SetDrawFunction((area, cairo, w, h) => cairo
                    //                         .AntiAlias(CairoAntialias.Best)
                    //                         .LineJoin(LineJoin.Miter)
                    //                         .LineCap(LineCap.Round)
                    //                         .Translate(w / 2.0, h / 2.0)
                    //                         .StrokePreserve()
                    //                         .ArcNegative(0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + progress * Math.PI * 2)
                    //                         .LineTo(0, 0)
                    //                         .SourceRgb(0.7, 0.7, 0.7)
                    //                         .Fill()
                    //                         .MoveTo(0, 0)
                    //                         .Arc(0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + progress * Math.PI * 2)
                    //                         .SourceRgb(0.3, 0.3, 0.3)
                    //                         .Fill()
                    //                     )
                    //                 )


    static async void MakeProgress(RevealerHandle revealer)
    {
        if (!revealer.IsChildRevealed())
            for (int i = 0; i < 1000; i++)
            {
                progress = i / 1000f;
                await Task.Delay(10);
                drawingArea.Ref.QueueDraw();
                progressBar.Ref.Fraction(progress);
            }
    }

    static float progress = 0.0f;

    static readonly ObjectRef<ToggleButtonHandle> progressStarter = new();
    static readonly ObjectRef<DrawingAreaHandle> drawingArea = new();
    static readonly ObjectRef<ProgressBarHandle> progressBar = new();
}

class ProgressWindowClass(GTypeEnum parent, string name, Func<nint, ProgressWindow> constructor)
    : SubClass<WindowHandle>(parent, name, constructor)
{
    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, "progress");
    }
}

class ProgressWindow(nint obj) : SubClassInst<WindowHandle>(obj)
{
    protected override void OnCreate()
    {
        Handle.InitTemplate();
        // Handle
        //     .GetTemplateChild<ButtonHandle, WindowHandle>("button1")
        //     ?.OnClicked(() => WriteLine("Button1 clicked"));
        // Handle
        //     .GetTemplateChild<ButtonHandle, WindowHandle>("quit")
        //     ?.OnClicked(() => Handle.CloseWindow());
    }
    protected override void OnFinalize() => WriteLine("Window finalized");
    protected override WindowHandle CreateHandle(nint obj) => new(obj);
}
