using GtkDotNet;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

static class Progress
{
    public static int Run()
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                        .Titlebar(
                            HeaderBar.New()
                            .PackEnd(
                                ToggleButton.New()
                                .Ref(progressStarter)
                                .IconName("open-menu-symbolic")
                            )
                            .PackEnd(
                                Revealer.New()
                                .SideEffect(r => progressStarter.Ref.BindProperty("active", r, "reveal-child", BindingFlags.Default))
                                .OnNotify("reveal-child", MakeProgress)                                                                
                                .TransitionType(RevealerTransition.SlideLeft)
                                .Child(
                                    MenuButton.New()
                                    .Popover(
                                        Popover.New()
                                        .Child(
                                            ProgressBar.New()
                                            .Ref(progressBar)
                                            .ShowText()
                                            .Fraction(.04)
                                        )
                                    )
                                    .Child(
                                        DrawingArea.New()
                                        .Ref(drawingArea)
                                        .SetDrawFunction((area, cairo, w, h) => cairo
                                            .AntiAlias(CairoAntialias.Best)
                                            .LineJoin(LineJoin.Miter)
                                            .LineCap(LineCap.Round)
                                            .Translate(w / 2.0, h / 2.0)
                                            .StrokePreserve()
                                            .ArcNegative(0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + progress * Math.PI * 2)
                                            .LineTo(0, 0)
                                            .SourceRgb(0.7, 0.7, 0.7)
                                            .Fill()
                                            .MoveTo(0, 0)
                                            .Arc(0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + progress * Math.PI * 2)
                                            .SourceRgb(0.3, 0.3, 0.3)
                                            .Fill()
                                        )
                                    )
                                )
                            )
                        )
                        .Title("Hello Gtk👍")
                        .DefaultSize(800, 600)
                        .Show())
            .Run(0, IntPtr.Zero);

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

