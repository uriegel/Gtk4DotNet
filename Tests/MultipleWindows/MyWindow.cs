using Gtk4DotNet;

class MyWindow : Window
{
    public MyWindow(Application app)
    {
        Construct();
        Title = "My custom Window";
        using var button = new Button("Kaputt");
        app.AddWindow(this);
    }   
}