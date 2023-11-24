using GtkDotNet;
using GtkDotNet.SafeHandles;
using LinqTools;

static class Drawing
{
    public static int Run()  
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                    .Title("Drawing Area👍")
                    .Child(
                        Frame.New()
                            .Child(DrawingArea
                                .New()
                                .Ref(drawingArea)
                                .SizeRequest(400, 400)
                                .SetDrawFunction((area, cairo, w, h) =>
                                    cairo
                                        .SideEffect(c => c.SetSourceSurface(surface, 0, 0))
                                        .Paint())
                                .OnResize(OnResize)
                                .AddController(
                                    GestureDrag    
                                        .New()
                                        .Button(MouseButton.Primary)
                                        .OnDragBegin(StrokeBegin)
                                )
                                .AddController(
                                    GestureDrag    
                                        .New()
                                        .Button(MouseButton.Primary)
                                        .OnDragUpdate(Stroke)
                                )
                                .AddController(
                                    GestureDrag    
                                        .New()
                                        .Button(MouseButton.Primary)
                                        .OnDragEnd(Stroke)
                                )
                                .AddController(
                                    GestureClick    
                                        .New()
                                        .Button(MouseButton.Secondary)
                                        .OnPressed((pressCount, x, y) => Reset())
                                )))
                    .Show())
            .Run(0, IntPtr.Zero);

    static void OnResize(DrawingAreaHandle da, int w, int h)
    {
        if (!surface.IsInvalid)
        {
            surface.Dispose();
            surface = new();
        }
        var nativeSurface = Native.GetSurface(da.GetNative());
        if (!nativeSurface.IsInvalid)
        {
            surface = nativeSurface.SurfaceCreateSimilar(CairoContent.Color, w, h);
            ClearSurface();
        }
    }

    static void ClearSurface()
        => Cairo
            .Create(surface)
            .SourceRgb(1, 1, 1)
            .Paint()
            .Dispose();

    static void Reset()
    {
        ClearSurface();
        drawingArea.Ref.QueueDraw();
    }

    static void DrawBrush(double x, double y)
        => Cairo
            .Create(surface)
            .Use(c => c
                .Rectangle(x - 3.0, y - 3.0, 6, 6)
                .Fill())
            .SideEffect(_ => drawingArea.Ref.QueueDraw());

    static void StrokeBegin(double x, double y)
    {
        startX = x;
        startY = y;
        DrawBrush(x, y);
    }

    static void Stroke(double x, double y)
        => DrawBrush(startX + x, startY + y);

    static readonly WidgetRef<DrawingAreaHandle> drawingArea = new();
    static SurfaceHandle surface = new();
    static double startX;
    static double startY;
}

