using System;
using System.Threading;
using System.Threading.Tasks;
using GtkDotNet;

var app = Application.New("org.gtk.example");

Action onActivate = () => 
{
    Application.RegisterResources();
    var cssProvider = CssProvider.New();
    cssProvider.CssProviderLoadFromResource("/org/gtk/example/style.css");
    StyleContext.AddProviderForDisplay(Display.GetDefault(), cssProvider, StyleProviderPriority.Fallback);

    var builder = Builder.FromResource("/org/gtk/example/window.ui");
    var window = Builder.GetObject(builder, "window");
    
    Window.SetApplication(window, app);
    GObject.Unref( builder);
    Widget.Show(window);
};

var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

