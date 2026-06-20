using System.ComponentModel;
using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        StyleContext.AddProviderForDisplay(
            Display.GetDefault(),
            CssProvider.New().FromResource("style"),
            StyleProviderPriority.Application);

        box.DataContext = dataContext;
        label1.SetBinding("label", nameof(WindowDataContext.Name));
        button1.OnClicked(async () =>
        {
            dataContext.Name = "Name was changed to John Doe";
            await Task.Delay(2000);
            dataContext.Name = "Name was changed back to URiegel";
        });
        buttonEmpty.OnClicked(() => dataContext.Name = "");
        buttonNull.OnClicked(() => dataContext.Name = null!);
        label2.SetBinding("label", nameof(WindowDataContext.Active));
        label3
            .Binding("label", nameof(WindowDataContext.Active), converter: b => (bool)b! ? "true" : "false")
            .SetBindingToCss("yellow", nameof(WindowDataContext.Active));
        checkBtn1.SetBinding("active", nameof(WindowDataContext.Active), BindingFlags.Bidirectional);
        checkBtn2.SetBinding("active", nameof(WindowDataContext.Active));
        trigger.OnToggled(b => dataContext.Active = b);
        editable
            .Binding("text", nameof(WindowDataContext.Name), BindingFlags.Bidirectional)
            .Notify("editing", () => Console.WriteLine("Editing..."));
    }

    readonly WindowDataContext dataContext = new();

    [Widget]
    readonly Widget box = null!;

    [Widget]
    readonly Widget label1 = null!;

    [Widget]
    readonly Widget label2 = null!;

    [Widget]
    readonly Widget label3 = null!;

    [Widget]
    readonly Button button1 = null!;

    [Widget]
    readonly Button buttonEmpty = null!;

    [Widget]
    readonly Button buttonNull = null!;

    [Widget(Name = "chk_1")]
    readonly Widget checkBtn1 = null!;

    [Widget(Name = "chk_2")]
    readonly Widget checkBtn2 = null!;

    [Widget(Name = "chk_trigger")]
    readonly CheckButton trigger = null!;

    [Widget]
    readonly Widget editable = null!;
}

class WindowDataContext : INotifyPropertyChanged
{
    public string Name
    {
        get => field ?? "";
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

    void OnChanged(string name) => PropertyChanged?.Invoke(this, new(name));
}