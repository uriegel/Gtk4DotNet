using System.Runtime.InteropServices;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public class GtkAction2 : FloatingObject
{
    public string Name { get; private set; } = null!;
    public string? Accelerator { get; private set; }

    public static GtkAction2 New(string name, Action action, string? accelerator = null)
    {
        var gAction = NewAction(name, null);
        gAction.CheckDiagnostics();
        gAction.Initialize(name, action, accelerator);
        return gAction;
    }

    public static GtkAction2 New(string name, bool initialState, Action<bool> stateChanged, string? accelerator = null)
    {
        var state = NewBool(initialState ? -1 : 0);
        var gAction = NewStatefulAction(name, null, state);
        // TODO free state
        gAction.CheckDiagnostics();
        gAction.Initialize(name, stateChanged, accelerator);
        return gAction;
    }

    public static GtkAction2 New(string name, string initialState, Action<string> stateChanged, string? accelerator = null)
    {
        var state = NewString(initialState ?? "");
        var gAction = NewStatefulAction(name, "s", state);
        // TODO free state
        gAction.CheckDiagnostics();
        gAction.Initialize(name, stateChanged, accelerator);
        return gAction;
    }

    public void Disconnect()
    {
        SignalDisconnect(this, signalId);
        GtkDelegates.Remove(id);
        IsFloating = false;
        Unref(GetInternalHandle());
        
    } 

    void Initialize(string name, Action action, string? accelerator = null)
    {
        Initialize(name, accelerator);
        id = GtkDelegates.Add(action);
        signalId = SignalConnectAction(this, "activate", Marshal.GetFunctionPointerForDelegate(action as Delegate), 0, 0);
    }

    void Initialize(string name, Action<bool> stateChanged, string? accelerator = null)
    {
        Initialize(name, accelerator);
        StateChanged = (a, s) =>
        {
            var state = HandleBoolState(a, s);
            stateChanged(state);
        };

        id = GtkDelegates.Add(StateChanged);
        signalId = SignalConnectAction(this, "change-state", Marshal.GetFunctionPointerForDelegate(StateChanged), 0, 0);
    }

    void Initialize(string name, Action<string> stateChanged, string? accelerator = null)
    {
        Initialize(name, accelerator);
        StateChanged = (a, s) =>
        {
            var state = HandleStringState(a, s);
            stateChanged(state);
        };

        id = GtkDelegates.Add(StateChanged);
        signalId = SignalConnectAction(this, "change-state", Marshal.GetFunctionPointerForDelegate(StateChanged), 0, 0);
    }

    void Initialize(string name, string? accelerator = null)
    {
        Name = name;
        Accelerator = accelerator;
        AddWeakRef(() 
            => GtkDelegates.Remove(id));
    }

    bool HandleBoolState(nint _, nint state)
    {
        ActionSetState(this, state);
        return GetBool(state) != 0;
        // TODO check free
    }

    string HandleStringState(IntPtr action, IntPtr state)
    {
        ActionSetState(this, state);
        var strptr = GetString(state, IntPtr.Zero);
        return Marshal.PtrToStringAuto(strptr) ?? "";
        // TODO check free
    }

    delegate void BoolStateChangedDelegate(bool newState);
    delegate void StringStateChangedDelegate(string newState);
    delegate void StateChangedDelegate(IntPtr action, IntPtr state);

    long id;
    long signalId;
    // TODO ???
    StateChangedDelegate StateChanged = (a, s) => { };

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new", CallingConvention = CallingConvention.Cdecl)]
    extern static GtkAction2 NewAction(string action, string? p);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_new_stateful", CallingConvention = CallingConvention.Cdecl)]
    extern static GtkAction2 NewStatefulAction(string action, string? p, nint state);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    extern static long SignalConnectAction(GtkAction2 action, string name, nint callback, nint obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_handler_disconnect", CallingConvention = CallingConvention.Cdecl)]
    extern static void SignalDisconnect(GtkAction2 action, long id);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static nint NewBool(int value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_new_string", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint NewString(string value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_set_state", CallingConvention = CallingConvention.Cdecl)]
    extern static void ActionSetState(GtkAction2 action, nint state);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetBool(nint value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetString(nint value, nint size);

    static readonly List<long> actionDelegateIds = [];
}


public class GtkAction1
{
    public GtkAction1(string actionName, Action action, string? accelerator = null)
    {
        Action = action;
        Name = actionName;
        Accelerator = accelerator;
    }

    public GtkAction1(string actionName, bool initialState, Action<bool> stateChanged, string? accelerator = null)
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
    public GtkAction1(string actionName, string initialState, Action<string> stateChanged, string? accelerator = null)
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


    [DllImport(Libs.LibGtk, EntryPoint = "g_simple_action_set_state", CallingConvention = CallingConvention.Cdecl)]
    extern static void ActionSetState(IntPtr action, IntPtr state);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_boolean", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetBool(IntPtr value);

    [DllImport(Libs.LibGtk, EntryPoint = "g_variant_get_string", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetString(IntPtr value, IntPtr size);

    readonly internal string Name;
    readonly internal string? Accelerator;
    readonly internal Action? Action;
    readonly internal string? StateParameterType;
    readonly internal object? State;
    readonly internal StateChangedDelegate StateChanged = (a, s) => { };
}

