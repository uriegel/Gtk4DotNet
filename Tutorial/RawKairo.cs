#define RAWKairo
#if RAWKairo

using System;
using GtkDotNet;

var app = Application.New("de.uriegel.test");
Application.Run(app, () =>
{
    var window = Window.New(GtkDotNet.WindowType.TopLevel);
    Window.SetTitle(window, "Kairo");

    var kairo = DrawingArea.New();
    DrawingArea.SetDrawFunction(kairo, (a, context, w, h, data) =>
    {
        Cairo.SetAntiAlias(context, GtkDotNet.CairoAntialias.Best);
        Cairo.SetLineJoin(context, GtkDotNet.LineJoin.Miter);
        Cairo.SetLineCap(context, GtkDotNet.LineCap.Round);
        Cairo.Translate(context, w / 2.0, h / 2.0);
        Cairo.StrokePreserve(context);
        Cairo.ArcNegative(context, 0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + 0.1 * Math.PI);
        Cairo.LineTo(context, 0, 0);
        Cairo.SetSourceRgb(context, 0.7, 0.7, 0.7);
        context.CairoFill();
        
        context.CairoMoveTo(0, 0);
        Cairo.Arc(context, 0, 0, (w < h ? w : h) / 2.0, -Math.PI / 2.0, -Math.PI / 2.0 + 0.1 * Math.PI);
        Cairo.SetSourceRgb(context, 0.3, 0.3, 0.3);
        context.CairoFill();
    });

    Window.SetChild(window, kairo);
    Application.AddWindow(app, window);
    Widget.Show(window);
});

delegate void DrawFunc(IntPtr widget, IntPtr context, IntPtr data);

#endif