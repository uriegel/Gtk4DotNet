namespace Gtk4DotNet;

public class GtkAction
{
    public string Name { get; }
    public Action? Action { get; }
    public Action<bool>? BoolAction { get; }
    public Action<string>? StringAction { get; }
    public bool InitialBoolState { get; }
    public string InitialStringState { get; } = "";

    public string? Accelerator { get; }
    public GtkAction(string name, Action action, string? accelerator = null)
    {
        Name = name;
        Action = action;
        Accelerator = accelerator;
    }   
    public GtkAction(string name, bool initialState, Action<bool> stateChanged, string? accelerator = null)
    {
        Name = name;
        InitialBoolState = initialState;
        BoolAction = stateChanged;
        Accelerator = accelerator;
    }
    public GtkAction(string name, string initialState, Action<string> stateChanged, string? accelerator = null)
    {
        Name = name;
        InitialStringState = initialState;
        StringAction = stateChanged;
        Accelerator = accelerator;
    }
}
