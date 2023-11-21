using GtkDotNet;
using GtkDotNet.SafeHandles;
using LinqTools;

using static System.Console;

static class Drawing
{
    public static int Run()  
        => Application
            .New("org.gtk.example")
            .OnActivate(app => 
                app
                    .NewWindow()
                    .Title("Drawing Area👍")
                    .Child(
                        Frame.New()
                            .Child(DrawingArea
                                .New()
                                .SizeRequest(400, 400)
                                .SetDrawFunction((area, cairo, w, h) => {

                                })
                                .AddController(
                                    GestureClick    
                                        .New()
                                        .Button(MouseButton.Secondary)
                                )))
                    .Show())
            .Run(0, IntPtr.Zero);
}

