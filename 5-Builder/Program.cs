using GtkDotNet;

using LinqTools;

return Application.Run("org.gtk.example", app => 
{
    Application.RegisterResources();
    GObjectRef
        .WithRef(Builder.FromResource("/org/gtk/example/window.ui"))
        .Use(builder =>
        {
            var window = builder.Value.GetObject("window");
            var button1 = builder
                            .Value
                            .GetObject("button1")
                            .SideEffect(b => b.SignalConnect("clicked", clicked));
            var button2 = builder
                            .Value
                            .GetObject("button2")
                            .SideEffect(b => b.SignalConnect("clicked", clicked));
            var quit = builder
                            .Value
                            .GetObject("quit")
                            .SideEffect(b => b.SignalConnect("clicked", () => window.Close()));
            window.SetApplication(app);
            window.Show();
        });
});

void clicked() => Console.WriteLine("Clicked button");