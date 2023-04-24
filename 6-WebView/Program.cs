using GtkDotNet;

using LinqTools;

return Application.Run("org.gtk.example", app =>
    Application
        .NewWindow(app)
        .SideEffect(w => w.SetTitle("Hello Web View👍"))
        .SideEffect(w => w.SetDefaultSize(800, 600))
        .SideEffect(w => w.SetChild(
            WebKit
                .New()
                .SideEffect(wk => 
                    wk
                        .GetSettings()
                        .SideEffect(s => s.SetBool("enable-developer-extras", true))
                    )
                .SideEffect(wk => wk.LoadUri("https://www.google.de"))
        ))
        .Show());



