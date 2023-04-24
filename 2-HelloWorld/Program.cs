using GtkDotNet;

using LinqTools;

return Application.Run("org.gtk.example", app =>
    Application
        .NewWindow(app)
        .SideEffect(w => w.SetTitle("Hello Gtk👍"))
        .SideEffect(w => w.SetDefaultSize(200, 200))
        .SideEffect(w => w.SetChild(
            Box
                .New(GtkDotNet.Orientation.Vertical, 0)
                .SideEffect(b => b.SetHAlign(Align.Center))
                .SideEffect(b => b.SetVAlign(Align.Center))
                .SideEffect(b => b.Append(
                    Button
                        .NewWithLabel("Hello Wörld")
                        .SideEffect(btn => btn.SignalConnect("clicked", () => Console.WriteLine("Clicked button")))
                ))
            ))
        .Show());




