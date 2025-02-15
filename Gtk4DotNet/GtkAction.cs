using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GtkAction
{
    public GtkAction(string actionName, Action action, string? accelerator = null)
    {
        Action = action;
        Name = actionName;
        Accelerator = accelerator;
    }
    public GtkAction(string actionName, bool initialState, Action<bool> stateChanged, string? accelerator = null)
    {
        Name = actionName;
        Accelerator = accelerator;
        StateParameterType = null;
        State = initialState;
        StateChanged = (a, s) => 
        {
            var state = HandleBoolState(a, s);
            stateChanged(state);
        };
    }
    public GtkAction(string actionName, string initialState, Action<string> stateChanged, string? accelerator = null)
    {
        Name = actionName;
        Accelerator = accelerator;
        StateParameterType = "s";
        State = initialState;
        StateChanged = (a, s) => 
        {
            var state = HandleStringState(a, s);
            stateChanged(state);
        };
    }

    static bool HandleBoolState(IntPtr action, IntPtr state)
    {
        ActionSetState(action, state);
        return GetBool(state) != 0;
    }
    
    static string HandleStringState(IntPtr action, IntPtr state)
    {
        ActionSetState(action, state);
        var strptr = GetString(state, IntPtr.Zero);
        return Marshal.PtrToStringAuto(strptr) ?? "";
    }

/*    public void SetBoolState(bool state)
    {
        if (action != IntPtr.Zero)
        {
            var var = Raw.Variant.NewBool(state);
            Raw.GtkAction.ActionSetState(action, var);
        }
    }

    public void SetStringState(string state)
    {
        if (action != IntPtr.Zero)
        {
            var var = Raw.Variant.NewString(state);
            Raw.GtkAction.ActionSetState(action, var);
        }
    }
*/
    internal IntPtr action { get; set; } = IntPtr.Zero;

    internal delegate void BoolStateChangedDelegate(bool newState);
    internal delegate void StringStateChangedDelegate(string newState);
    
    internal delegate void StateChangedDelegate(IntPtr action, IntPtr state);


    [DllImport(Libs.LibGtk, EntryPoint="g_simple_action_set_state", CallingConvention = CallingConvention.Cdecl)]
    extern static void ActionSetState(IntPtr action, IntPtr state);

    [DllImport(Libs.LibGtk, EntryPoint="g_variant_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetBool(IntPtr value);
    
    [DllImport(Libs.LibGtk, EntryPoint="g_variant_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetString(IntPtr value, IntPtr size);

    readonly internal string Name;
    readonly internal string? Accelerator;
    readonly internal Action? Action;
    readonly internal string? StateParameterType;
    readonly internal object? State;
    readonly internal StateChangedDelegate StateChanged = (a, s) => {};
}

