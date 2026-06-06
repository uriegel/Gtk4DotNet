namespace Gtk4DotNet;

public class ApplicationWindow : Window // , IActionMap
{
    public ApplicationWindow() : base() { }
    
    public ApplicationWindow(WindowBuilder builder) : base()
    {
        builder.Builder.GetWindow(this, builder.Window);
        SetApplication(this, builder.Application);
    }

    public ApplicationWindow(nint obj) : base() => SetInternalHandle(obj);

    internal ApplicationWindow(Widget widget) : base() => handle = widget.TakeHandle();
}


