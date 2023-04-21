using System;
using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var window = Application.NewWindow(app);
    Window.SetTitle(window, "Drawing Area👍");
    
    var frame = Frame.New();
    Window.SetChild(window, frame);

    var drawingArea = DrawingArea.New();
    Widget.SetSizeRequest(drawingArea, 100, 100);

    Frame.SetChild(frame, drawingArea);

    var surface = IntPtr.Zero;

    void draw(IntPtr area, IntPtr cairo, int w, int h, IntPtr zero) 
    {
        Cairo.SetSourceSurface(cairo, surface, 0, 0);
        Cairo.Paint(cairo);
    }

    void clearSurface()
    {
        var cairo = Cairo.Create(surface);
        Cairo.SetSourceRgb(cairo, 1, 1, 1);
        Cairo.Paint(cairo);
        Cairo.Destroy(cairo);
    }


    void resize(IntPtr widget, int w, int h, IntPtr zero)
    {
        if (surface != IntPtr.Zero)
        {
            Cairo.SurfaceDestroy(surface);
            surface = IntPtr.Zero;
        }
        var nativeSurface = Native.GetSurface(Widget.GetNative(widget));
        if (nativeSurface != IntPtr.Zero)
        {
            surface = Cairo.SurfaceCreateSimilar(nativeSurface, CairoContent.Color, Widget.GetWidth(widget), Widget.GetHeight(widget));
            clearSurface();
        }
    }

    DrawingArea.SetDrawFunction(drawingArea, draw);
    Gtk.SignalConnectAfter<Resize>(drawingArea, "resize", resize);

    var startX = 0.0;
    var startY = 0.0;

    void drawBrush(IntPtr widget, double x, double y)
    {
        var cairo = Cairo.Create(surface);
        Cairo.Rectangle(cairo, x-3.0, y-3.0, 6, 6);
        Cairo.Fill(cairo);
        Cairo.Destroy(cairo);
        Widget.QueueDraw(widget);
    }

    void dragBegin(IntPtr gesture, double x, double y, IntPtr zero) 
    { 
        startX = x;
        startY = y;
        drawBrush(drawingArea, x, y);
    }

    void dragUpdate(IntPtr gesture, double x, double y, IntPtr zero) 
        => drawBrush(drawingArea, startX+x, startY+y); 
    
    void dragEnd(IntPtr gesture, double x, double y, IntPtr zero) 
        => drawBrush(drawingArea, startX+x, startY+y); 

    void pressed(IntPtr gesture, int pressCount, double x, double y, IntPtr zero) 
    {
        clearSurface();
        Widget.QueueDraw(drawingArea);
    }

    var gestureDrag = GestureDrag.New();
    GestureSingle.SetButton(gestureDrag, MouseButton.Primary);
    Widget.AddController(drawingArea, gestureDrag);
    Gtk.SignalConnect<Drag>(gestureDrag, "drag-begin", dragBegin);
    Gtk.SignalConnect<Drag>(gestureDrag, "drag-update", dragUpdate);
    Gtk.SignalConnect<Drag>(gestureDrag, "drag-end", dragEnd);

    var press = GestureClick.New();
    GestureSingle.SetButton(press, MouseButton.Secondary);
    Widget.AddController(drawingArea, press);
    Gtk.SignalConnect<Pressed>(press, "pressed", pressed);

    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

delegate void Resize(IntPtr widget, int w, int h, IntPtr zero);
delegate void Drag(IntPtr gesture, double x, double y, IntPtr zero);
delegate void Pressed(IntPtr gesture, int pressCount, double x, double y, IntPtr zero);