using Gtk4DotNet;

class MyWindow : Window
{
    public MyWindow(Application app)
    {
        Construct();
        Title = "My custom Window";
        app.AddWindow(this);
    }   
}