using GtkDotNet;

var app = Application.New("org.gtk.example");
Action onActivate = () => 
{
    var label = Label.New("Label");
    GObject.RefSink(label);
    GObject.AddWeakRef(label, (_, obj) => Console.WriteLine("Label finalized"));
    GObject.Unref(label);

    var labelRef = new GObjectRef(Label.New("Label 2"));
    GObject.RefSink(labelRef.Value);
    GObject.AddWeakRef(labelRef.Value, (_, obj) => Console.WriteLine("Label 2 finalized"));
    labelRef.Dispose();

    var test = GManaged<Test>.New(new ("First object"));
    GManaged<Test>.SetValue(test, new ("Second object"));
    GObject.Unref(test);

    var label3 = Label.New("Label 3");
    GObject.RefSink(label3);
    GObject.AddWeakRef(label3, (_, obj) => Console.WriteLine("Label 3 finalized"));
    var label4 = Label.New("Label 4");
    GObject.RefSink(label4);
    GObject.AddWeakRef(label4, (_, obj) => Console.WriteLine("Label 4 finalized"));

    test = GManaged<GObjectRef>.New(new GObjectRef(label3));
    GManaged<GObjectRef>.SetValue(test, new GObjectRef(label4));
    GObject.Unref(test);
};

var status = Application.Run(app, onActivate);
GObject.Unref(app);

GC.Collect();
GC.Collect();
Thread.Sleep(1000);

return status;

class Test
{
    public string Text { get;}

    public Test(string text) => Text = text;
    ~Test()
    {
        Console.WriteLine("Finalisiert");
    }
}