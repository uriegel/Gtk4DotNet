using GtkDotNet;

using LinqTools;

var surface = IntPtr.Zero;
var startX = 0.0;
var startY = 0.0;

return Application.Run("org.gtk.example", app =>
    Application
        .NewWindow(app)
        .SideEffect(win => win.SetTitle("Drawing Area👍"))
        .SideEffect(win => win.SetChild(
            Frame
                .New()
                .SideEffect(frm => frm.FrameSetChild(
                    DrawingArea
                        .New()
                        .SideEffect(da => da.SetSizeRequest(400, 400))
                        .SideEffect(da => da.SetDrawFunction((area, cairo, w, h, zero) =>
                            cairo.SideEffect(c => c.SetSourceSurface(surface, 0, 0))
                            .Paint()))
                        .SideEffect(da => da.SignalConnectAfter<ResizeFunc>("resize", (widget, w, h, zero) =>
                        {
                            if (surface != IntPtr.Zero)
                            {
                                surface.SurfaceDestroy();
                                surface = IntPtr.Zero;
                            }
                            var nativeSurface = Native.GetSurface(widget.GetNative());
                            if (nativeSurface != IntPtr.Zero)
                            {
                                surface = Cairo.SurfaceCreateSimilar(nativeSurface, CairoContent.Color, widget.GetWidth(), widget.GetHeight());
                                ClearSurface();
                            }
                        }))
                        .SideEffect(da => da.AddController(
                            GestureDrag
                                .New()
                                .SideEffect(g => g.GestureSingleSetButton(MouseButton.Primary))
                                .SideEffect(g => g.SignalConnect<DragFunc>("drag-begin", (gesture, x, y, zero) =>
                                {
                                    startX = x;
                                    startY = y;
                                    DrawBrush(da, x, y);
                                }))
                                .SideEffect(g => g.SignalConnect<DragFunc>("drag-update", (gesture, x, y, zero) =>
                                    DrawBrush(da, startX + x, startY + y)))
                                .SideEffect(g => g.SignalConnect<DragFunc>("drag-end", (gesture, x, y, zero) =>
                                    DrawBrush(da, startX + x, startY + y)))
                        ))
                        .SideEffect(da => da.AddController(
                            GestureClick
                                .New()
                                .SideEffect(g => g.GestureSingleSetButton(MouseButton.Secondary))
                                .SideEffect(g => g.SignalConnect<PressedFunc>("pressed", (gesture, pressCount, x, y, zero) =>
                                {
                                    ClearSurface();
                                    da.QueueDraw();
                                }

                            ))
                        ))
                    ))
                ))
            .Show());

void ClearSurface()
{
    var cairo = Cairo.Create(surface);
    Cairo.SetSourceRgb(cairo, 1, 1, 1);
    Cairo.Paint(cairo);
    cairo.CairoDestroy();
}

void DrawBrush(IntPtr widget, double x, double y)
{
    var cairo = Cairo.Create(surface);
    Cairo.Rectangle(cairo, x-3.0, y-3.0, 6, 6);
    cairo.CairoFill();
    cairo.CairoDestroy();
    widget.QueueDraw();
}



