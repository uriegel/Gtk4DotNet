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
                    .SubClass(new ProgressDisplayClass(GTypeEnum.Revealer, "ProgressDisplay", p => new ProgressDisplay(p)))
                    .SideEffect(a =>
                        GObject.New<WindowHandle>("ProgressWindow".TypeFromName())
                        .SetApplication(app)
                        .Show()))
            .Run(0, IntPtr.Zero);
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
    protected override void OnCreate() => Handle.InitTemplate();
    protected override void OnFinalize() => WriteLine("Window finalized");
    protected override WindowHandle CreateHandle(nint obj) => new(obj);
}

class ProgressDisplayClass(GTypeEnum parent, string name, Func<nint, ProgressDisplay> constructor)
    : SubClass<RevealerHandle>(parent, name, constructor) {}

class ProgressDisplay(nint obj) : SubClassInst<RevealerHandle>(obj)
{
    protected override void OnCreate()
    {
        Handle.OnNotify("reveal-child", MakeProgress);

        async void MakeProgress(RevealerHandle revealer)
        {
            activeId++;
            if (!revealer.IsChildRevealed())
            {
                var id = activeId;
                var progressBar = Handle.GetTemplateChild<ProgressBarHandle, RevealerHandle>("progress_bar");
                var drawingArea =
                    Handle.
                        GetTemplateChild<DrawingAreaHandle, RevealerHandle>("progress_area")
                            ?.SetDrawFunction((area, cairo, w, h) =>
                                cairo
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
                                    .Fill());
                for (int i = 0; i < 1000 && id == activeId && !closing; i++)
                {
                    progress = i / 1000f;
                    await Task.Delay(10);
                    if (closing || id != activeId)
                        return;
                    drawingArea?.QueueDraw();
                    progressBar?.Fraction(progress);
                }
            }
        }
    }

    protected override void OnFinalize()
    {
        closing = true;
        WriteLine("Revealer finalized");
    }

    protected override RevealerHandle CreateHandle(nint obj) => new(obj);

    float progress = 0.0f;
    bool closing;
    int activeId;
}
