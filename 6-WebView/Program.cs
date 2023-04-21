using System;
using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var window = Application.NewWindow(app);
    Window.SetTitle(window, "Hello Web View👍");
    Window.SetDefaultSize(window, 200, 200);

    var webView = WebKit.New();
    var settings = WebKit.GetSettings(webView);
    GObject.SetBool(settings, "enable-developer-extras", true);
    Window.SetChild(window, webView);    
    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;


