using Gtk4DotNet;

class ProgressDisplay : Revealer
{
    public ProgressDisplay(Builder builder, string name) : base(builder, name)
    {
        AddCssClass("custom-accent");
        drawingArea.SetDrawFunction(Draw);
        this["reveal-child"].OnNotify += MakeProgress;
        starter.BindProperty("active", this, "reveal-child", BindingFlags.Bidirectional);
        OnFinalize(async () =>
        {
            closing = true;
            await Task.Delay(400);
        });
    }

    void Draw(DrawingArea area, Cairo cairo, int w, int h)
    {
        var color = GetStyleContext().GetColor().ToSrgb();
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
    }

    async void MakeProgress()
    {
        activeId++;
        if (!IsRevealed)
        {
            var id = activeId;
            for (int i = 0; i < 1000 && id == activeId && !closing; i++)
            {
                progress = i / 1000f;
                await Task.Delay(10);
                if (closing || id != activeId)
                    return;
                drawingArea.QueueDraw();
                progressBar.Fraction = progress;                
            }
            await Task.Delay(5000);
            IsRevealed = false;
        }
    }

    [Widget(Name = "progress_bar")]
    ProgressBar progressBar = null!;

    [Widget(Name = "progress_area")]
    DrawingArea drawingArea = null!;

    [Widget]
    Widget starter = null!;

    float progress = 0.0f;
    
    bool closing;
    int activeId;
}

