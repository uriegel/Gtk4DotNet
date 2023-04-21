module GtkTutorial

open GtkDotNet

let app = Application.New "org.gtk.example"
let onActivate () =
    let window = Application.NewWindow app
    Window.SetTitle (window, "Hello Gtk👍")
    Window.SetDefaultSize (window, 200, 200)
    Widget.Show window

let status = Application.Run (app, onActivate)
GObject.Unref app

