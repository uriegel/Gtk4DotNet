module GtkTutorial

open GtkDotNet

let clicked () =
    printfn "Clicked button"

let app = Application.New "org.gtk.example"
let onActivate () =
    let window = Application.NewWindow app
    Window.SetTitle (window, "Hello Gtk👍")
    Window.SetDefaultSize (window, 200, 200)

    let box = Box.New (GtkDotNet.Orientation.Vertical, 0)
    Widget.SetHAlign (box, GtkDotNet.Align.Center) 
    Widget.SetVAlign (box, GtkDotNet.Align.Center)

    Window.SetChild (window, box) |> ignore

    let button = Button.NewWithLabel "Hello Wörld"
    Gtk.SignalConnect<System.Action> (button, "clicked", clicked)

    Box.Append (box, button)

    Widget.Show window

let status = Application.Run (app, onActivate)
GObject.Unref app

