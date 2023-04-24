using GtkDotNet;

using LinqTools;

return Application.Run("org.gtk.example", app => 
    Application
        .NewWindow(app)
        .SideEffect(w => w.SetTitle("Hello Gtk👍"))
        .SideEffect(w => w.SetDefaultSize(1200, 1200))
        .Show());





