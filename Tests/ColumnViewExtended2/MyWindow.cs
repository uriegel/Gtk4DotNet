using Gtk4DotNet;

class MyWindow : ApplicationWindow
{
    public MyWindow(WindowBuilder builder) : base(builder)
    {
        StyleContext.AddProviderForDisplay(
            Display.GetDefault(),
            CssProvider.New().FromResource("style"),
            StyleProviderPriority.Application);

        paned["position"].OnNotify += OnPosition;
    }

    void OnPosition()
    {
        // if (columnviewLeft.Width == 0 && columnviewRight.Width == 0)
        //     return;
        // OnWidth(columnviewLeft, ref implodedLeft);
        // OnWidth(columnviewRight, ref implodedRight);
    }

    [Widget]
    readonly Paned paned = null!;

    [Widget(Template = "columnview")]
    readonly MyColumnView leftView = null!;

    [Widget(Template = "columnview")]
    readonly MyColumnView rightView = null!;
}
