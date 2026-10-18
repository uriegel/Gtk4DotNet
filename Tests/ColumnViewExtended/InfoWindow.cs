using Gtk4DotNet;

class InfoWindow : Window
{
    public InfoWindow(Application app, Builder builder) : base(builder, "window")
    {
        app.AddWindow(this);

        var infos = Gtk.GtkInfo();
        label.Text = string.Join("\r\n", infos);

        label.OnFinalize(async () =>
        {
            Console.WriteLine("Eliminiert");
            await Task.Delay(500);
            GC.Collect();
            GC.Collect();
        });

        AddTest();
        GC.Collect();
        GC.Collect();
        GC.Collect();
    }

    void AddTest()
    {
        var test = new Test();
        label.SetManagedData("key", test);
    }

    [Widget]
    readonly Label label = null!;
}

class Test()
{
    ~Test()
    {
        Console.WriteLine("Test eliminiert");
    }
}