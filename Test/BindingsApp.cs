using System.ComponentModel;
using GtkDotNet;

static class BindingsApp
{
    public static int Run()
    {
        var dataContext = new DataContext()
        {
            Name = "URiegel"
        };

        return Application
            .NewAdwaita("org.gtk.example")
            .OnActivate(app =>
                app
                    .NewWindow()
                    .Title("Hello Gtk Bindings👍")
                    .Child(Box
                        .New(Orientation.Vertical)
                        .DataContext(dataContext)
                        .Append(Box
                            .New(Orientation.Horizontal)
                            .Spacing(10)
                            .Margin(5)
                            .Append(Label
                                .New()
                                .HAlign(Align.Start)
                                .HExpand(true)
                                .Binding("label", "Name", BindingFlags.Default))
                            .Append(Button
                                .NewWithLabel("Change")
                                .OnClicked(async () =>
                                    {

                                        // TODO on enter in EditableLabel
                                            // TODO notify:editing


                                        var ct = ContentType.Guess("/home/test/zwe.dll");
                                        using var icon = ContentType.GetIcon(ct!);
                                        var waht = icon.Names();




                                        dataContext.Name = "Name was changed to John Doe";
                                        await Task.Delay(2000);
                                        dataContext.Name = "Name was changed back to URiegel";
                                    }))
                            .Append(Button
                                .NewWithLabel("To null")
                                .OnClicked(() => dataContext.Name = null)))
                        .Append(Box
                            .New(Orientation.Horizontal)
                            .Spacing(10)
                            .Margin(5)
                            .Append(Label
                                .New()
                                .HAlign(Align.Start)
                                .HExpand(true)
                                .Binding("label", "Active", BindingFlags.Default)))
                        .Append(Box
                            .New(Orientation.Horizontal)
                            .Spacing(10)
                            .Margin(5)
                            .Append(Label
                                .New()
                                .HAlign(Align.Start)
                                .HExpand(true)
                                .Binding("label", "Active", BindingFlags.Default, b => (bool)b! ? "true" : "false")))
                        .Append(Box
                            .New(Orientation.Horizontal)
                            .Spacing(10)
                            .Margin(5)
                            .Append(EditableLabel
                                .New()
                                .HAlign(Align.Start)
                                .HExpand(true)
                                .Binding("text", "Name", BindingFlags.Bidirectional)))
                        .Append(Box
                            .New(Orientation.Horizontal)
                            .Spacing(10)
                            .Margin(5)
                            .Append(CheckButton
                                .NewWithLabel("Binding")
                                .HAlign(Align.Start)
                                .HExpand(true)  
                                .Binding("active", "Active", BindingFlags.Bidirectional)))
                        .Append(Box
                            .New(Orientation.Horizontal)
                            .Spacing(10)
                            .Margin(5)
                            .Append(CheckButton
                                .NewWithLabel("Binding")
                                .HAlign(Align.Start)
                                .HExpand(true)  
                                .Binding("active", "Active", BindingFlags.Default))
                            .Append(CheckButton
                                .NewWithLabel("Trigger")
                                .OnToggled(b => dataContext.Active = b.IsActive()))))
                    .Show())
            .Run(0, IntPtr.Zero);
    }
}

class DataContext : INotifyPropertyChanged
{
    public string? Name
    {
        get => _Name;
        set
        {
            _Name = value;
            OnChanged(nameof(Name));
        }
    }
    string? _Name;

    public bool Active
    {
        get => _Active;
        set
        {
            _Active = value;
            OnChanged(nameof(Active));
        }
    }
    bool _Active;

    public event PropertyChangedEventHandler? PropertyChanged;

    // TODO to main thread
    void OnChanged(string name) => PropertyChanged?.Invoke(this, new(name));
}