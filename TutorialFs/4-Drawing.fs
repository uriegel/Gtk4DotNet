module GtkTutorial

open System
open GtkDotNet

type Resize =
   delegate of
      wi: nativeint *
      w : int *
      h : int *
      z : nativeint
       -> unit

type DragAction =
   delegate of
      gesture: nativeint *
      x      : double *
      y      : double *
      zero   : nativeint
       -> unit

type Pressed =
   delegate of
      gesture   : nativeint *
      pressCount: int *
      x         : double *
      y         : double *
      zero      : nativeint
       -> unit


let app = Application.New "org.gtk.example"
let onActivate () =
    let window = Application.NewWindow app
    Window.SetTitle (window, "Drawing Area👍")

    let frame = Frame.New ()
    Window.SetChild (window, frame) |> ignore

    let drawingArea = DrawingArea.New ()
    Widget.SetSizeRequest (drawingArea, 100, 100)

    Frame.SetChild (frame, drawingArea)

    let mutable surface = IntPtr.Zero

    let draw (area: nativeint) (cairo: nativeint) (w: int) (h: int) (nil: nativeint) =
        Cairo.SetSourceSurface (cairo, surface, 0, 0)
        Cairo.Paint cairo

    let clearSurface () =
        let cairo = Cairo.Create surface
        Cairo.SetSourceRgb (cairo, 1, 1, 1);
        Cairo.Paint cairo
        Cairo.Destroy cairo

    let resize (widget: nativeint) (w: int) (h: int) (zero: nativeint) =
        if surface <> IntPtr.Zero then
            Cairo.SurfaceDestroy surface
            surface <- IntPtr.Zero
        let nativeSurface = Native.GetSurface (Widget.GetNative widget)
        if nativeSurface <> IntPtr.Zero then
            surface <- Cairo.SurfaceCreateSimilar(nativeSurface, CairoContent.Color, Widget.GetWidth(widget), Widget.GetHeight(widget))
            clearSurface ()

    DrawingArea.SetDrawFunction (drawingArea, draw)
    Gtk.SignalConnectAfter<Resize>(drawingArea, "resize", resize);
    
    let mutable startX = 0.0
    let mutable startY = 0.0

    let drawBrush (widget: nativeint) (x: double) (y: double) =
        let cairo = Cairo.Create surface
        Cairo.Rectangle (cairo, x-3.0, y-3.0, 6, 6)
        Cairo.Fill cairo
        Cairo.Destroy cairo
        Widget.QueueDraw widget

    let dragBegin (gesture: nativeint) (x: double) (y: double) (nil: nativeint) = 
        startX <- x
        startY <- y
        drawBrush drawingArea x y 

    let dragUpdate (gesture: nativeint) (x: double) (y: double) (nil: nativeint) = 
        drawBrush drawingArea (startX+x) (startY+y) 
    
    let dragEnd (gesture: nativeint) (x: double) (y: double) (nil: nativeint) = 
        drawBrush drawingArea (startX+x) (startY+y) 

    let pressed (gesture: nativeint) (pressCount: int) (x: double) (y: double) (zero: nativeint) =
        clearSurface ()
        Widget.QueueDraw drawingArea 

    let gestureDrag = GestureDrag.New();
    GestureSingle.SetButton(gestureDrag, MouseButton.Primary);
    Widget.AddController(drawingArea, gestureDrag);
    Gtk.SignalConnect<DragAction>(gestureDrag, "drag-begin", dragBegin);
    Gtk.SignalConnect<DragAction>(gestureDrag, "drag-update", dragUpdate);
    Gtk.SignalConnect<DragAction>(gestureDrag, "drag-end", dragEnd);

    let press = GestureClick.New();
    GestureSingle.SetButton(press, MouseButton.Secondary);
    Widget.AddController(drawingArea, press);
    Gtk.SignalConnect<Pressed>(press, "pressed", pressed);

    Widget.Show window

    let closeWindow () = 
        if surface <> IntPtr.Zero then
            Cairo.SurfaceDestroy surface            

    Gtk.SignalConnect<System.Action> (window, "destroy", closeWindow)

let status = Application.Run (app, onActivate)
GObject.Unref app

