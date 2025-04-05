using CsTools.Extensions;
using GtkDotNet;
using GtkDotNet.SafeHandles;
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
                    .CustomWindow("ProgressWindow")
                        .SideEffect(_ => StyleContext
                            .AddProviderForDisplay(Display.GetDefault(),
                                CssProvider.New()
                                    .FromResource("progressstyle"), StyleProviderPriority.Application))
                        .Show())
            .Run(0, IntPtr.Zero);
}

class ProgressWindowClass(GTypeEnum parent, string name, Func<nint, ProgressWindow> constructor)
    : SubClass<ApplicationWindowHandle>(parent, name, constructor)
{
    protected override void ClassInit(nint cls, nint _)
    {
        base.ClassInit(cls, _);
        InitTemplateFromResource(cls, "progress");
    }
}

class ProgressWindow(nint obj) : SubClassInst<ApplicationWindowHandle>(obj)
{
    protected override void OnCreate() => Handle.InitTemplate();
    protected override void OnFinalize() => WriteLine("Window finalized");
    protected override ApplicationWindowHandle CreateHandle(nint obj) => new(obj);
}

class ProgressDisplayClass(GTypeEnum parent, string name, Func<nint, ProgressDisplay> constructor)
    : SubClass<RevealerHandle>(parent, name, constructor) {}

class ProgressDisplay(nint obj) : SubClassInst<RevealerHandle>(obj)
{
    protected override async void OnCreate()
    {
        await Task.Delay(1);
        var progressBar = Handle.GetTemplateChild<ProgressBarHandle, RevealerHandle>("progress_bar");
        var drawingArea =
            Handle
                .CssClass("custom-accent")
                .GetTemplateChild<DrawingAreaHandle, RevealerHandle>("progress_area")
                    ?.SetDrawFunction((area, cairo, w, h) =>
                    {
                        var color = Handle.GetStyleContext().GetColor().ToSrgb();
                        cairo
                            .AntiAlias(CairoAntialias.Best)
                            .LineCap(LineCap.Round)
                            .LineWidth(3.0)
                            .SourceRgba(color.Red, color.Green, color.Blue, 0.2)
                            .Arc(w / 2.0, h / 2.0, (w < h ? w : h) / 2.0 - 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + Math.PI * 2)
                            .Stroke()
                            .AntiAlias(CairoAntialias.Best)
                            .LineCap(LineCap.Round)
                            .LineWidth(3.0)
                            .SourceRgba(color.Red, color.Green, color.Blue, color.Alpha)
                            .Arc(w / 2.0, h / 2.0, (w < h ? w : h) / 2.0 - 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + progress * Math.PI * 2)
                            .Stroke();
                    });

        Handle.OnNotify("reveal-child", MakeProgress);

        async void MakeProgress(RevealerHandle revealer)
        {

            activeId++;
            if (!revealer.IsChildRevealed())
            {
                var id = activeId;
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
