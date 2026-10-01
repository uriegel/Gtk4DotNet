using Gtk4DotNet;

class MyWindow : Window
{
    public MyWindow(Application app) 
    {
        Title = "My custom Window";
        app.AddWindow(this);
    }   
}