using System.ComponentModel;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        box.DataContext = dataContext;
        label1.SetBinding("label", nameof(WindowDataContext.Name));
        button1.OnClicked(async () =>
        {
            dataContext.Name = "Name was changed to John Doe";
            await Task.Delay(2000);
            dataContext.Name = "Name was changed back to URiegel";
        });
        buttonNull.OnClicked(() => dataContext.Name = null);
    }
    
    readonly WindowDataContext dataContext = new();

    [Widget(Name = "box")]
    readonly Widget box = null!;
    [Widget(Name = "label1")]
    readonly Widget label1 = null!;
    [Widget(Name = "button1")]
    readonly Button button1 = null!;
    [Widget(Name = "buttonNull")]
    readonly Button buttonNull = null!;
}

class WindowDataContext : INotifyPropertyChanged
{
    public string? Name
    {
        get;
        set
        {
            field = value;
            OnChanged(nameof(Name));
        }
    }

    public bool Active
    {
        get;
        set
        {
            field = value;
            OnChanged(nameof(Active));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // TODO to main thread
    void OnChanged(string name) => PropertyChanged?.Invoke(this, new(name));
}