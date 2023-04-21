//#define ProgramBuilder
#if ProgramBuilder

using System;
using GtkDotNet.Raw;

Console.WriteLine("Hello Gtk 4");

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var val1 = GIntType.New();
    var val2 = GIntType.New();
    var value = GIntType.GetValue(val1);
    value = GIntType.GetValue(val2);
    GIntType.SetValue(val1, 9991);
    GIntType.SetValue(val2, 12345);
    value = GIntType.GetValue(val1);
    value = GIntType.GetValue(val2);
    
    var builder = Builder.New();
    Builder.AddFromFile(builder, "builder.ui", IntPtr.Zero);

    var window = Builder.GetObject(builder, "window");
    Window.SetApplication(window, app);
    
    Widget.Show(window);
    GObject.Unref(builder);
};


var status = Application.Run(app, onActivate);

GObject.Unref(app);

return status;

#endif