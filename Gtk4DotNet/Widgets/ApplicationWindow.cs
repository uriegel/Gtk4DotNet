namespace Gtk4DotNet;

public class ApplicationWindow : Window, IActionMap
{
    public ApplicationWindow() : base()
    {
        AddWeakRef((this as IActionMap).FreeActions);
    }

    public ApplicationWindow(WindowBuilder builder) : base(builder.Builder, builder.Window)
    {
        SetApplication(this, builder.Application);
        AddWeakRef((this as IActionMap).FreeActions);
    }

    #region IActionMap

    public List<GtkAction> GetActionList() => actionList;
    readonly List<GtkAction> actionList = [];

    #endregion
}


