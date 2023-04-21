module GtkTutorial

open GtkDotNet

let clicked () =
    printfn "Clicked button"

let app = Application.New "org.gtk.example"
let onActivate () =
    let window = Application.NewWindow app
    Window.SetTitle (window, "Hello Gtk👍")
    
    let grid = Grid.New ()
    Window.SetChild (window, grid) |> ignore

    let button = Button.NewWithLabel "Button 1"
    Gtk.SignalConnect<System.Action> (button, "clicked", clicked)
    Grid.Attach (grid, button, 0, 0, 1, 1)

    let button = Button.NewWithLabel "Button 2"
    Gtk.SignalConnect<System.Action> (button, "clicked", clicked)
    Grid.Attach (grid, button, 1, 0, 1, 1)

    let button = Button.NewWithLabel "Quit"
    Gtk.SignalConnect<System.Action> (button, "clicked", fun () -> (Window.Close window))
    Grid.Attach (grid, button, 0, 1, 2, 1)

    Widget.Show window

let status = Application.Run (app, onActivate)
GObject.Unref app

