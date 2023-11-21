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
                                .SizeRequest(400, 400)
                                .SetDrawFunction((area, cairo, w, h) => 
                                    cairo
                                        .SideEffect(c => c.SetSourceSurface(surface, 0, 0))
                                        .Paint())
                                .OnResize(OnResize)
                                .AddController(
                                    GestureClick    
                                        .New()
                                        .Button(MouseButton.Secondary)
                                        .OnPressed((pressCount, x, y) => ClearSurface())
                                )))
                    .Show())
            .Run(0, IntPtr.Zero);

    static SurfaceHandle surface = new();

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
}

