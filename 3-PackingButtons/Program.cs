using GtkDotNet;

using LinqTools;

return Application.Run("org.gtk.example", app =>
    Application
        .NewWindow(app)
        .SideEffect(win => win.SetTitle("Hello Gtk👍"))
        .SideEffect(win => win.SetChild(
            Grid
                .New()
                .SideEffect(g => g.Attach(
                    Button
                        .NewWithLabel("Button 1")
                        .SideEffect(btn => btn.SignalConnect("clicked", clicked))
                    , 0, 0, 1, 1))
                .SideEffect(g => g.Attach(
                    Button
                        .NewWithLabel("Button 2")
                        .SideEffect(btn => btn.SignalConnect("clicked", clicked))
                    , 1, 0, 1, 1))
                .SideEffect(g => g.Attach(
                    Button
                        .NewWithLabel("Quit")
                        .SideEffect(btn => btn.SignalConnect("clicked", () => win.Close()))
                    , 0, 1, 2, 1))
        ))
        .Show());

void clicked() => Console.WriteLine("Clicked button");    



