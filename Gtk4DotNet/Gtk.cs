using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

// TODO Extended Samples in Branch "OldVersion"

public static class Gtk
{
    /// <summary>
    /// Starts a new GTK application for use in a non GTK (console) app, see example '7-Non Gtk app'
    /// </summary>
    public static void Start()
        => new Thread(() =>
        {
            nonGtkApp = Application.New("de.uriegel.gtk4dotnet");
            nonGtkApp.OnActivate(a => nonGtkWindow = a.NewWindow());
            nonGtkApp.Run(0, IntPtr.Zero);
            nonGtkApp.Dispose();
        }).Start();

    /// <summary>
    /// Stops the GTK application started with 'Start'
    /// </summary>
    public static void Stop()
        => Dispatch(() => {
            nonGtkWindow?.Close();
            nonGtkApp?.Quit();
        });

    public static Task Dispatch(Action action, bool highPriority = false)
        => Dispatch(action, highPriority ? 100 : 200);

    public static Task Dispatch(Action action, int priority)
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        BeginInvoke(priority, () =>
        {
            try
            {
                action();
                tcs.TrySetResult();
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        });
        return tcs.Task;
    }

    public static Task<T> Dispatch<T>(Func<T> action, bool highPriority = false)
        => Dispatch(action, highPriority ? 100 : 200);

    public static Task<T> Dispatch<T>(Func<T> action, int priority)
    {
        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        BeginInvoke(priority, () => 
        {
            try
            {
                tcs.TrySetResult(action());
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        });
        return tcs.Task;
    }

    /// <summary>
    /// Run the specified Action in the main GTK thread
    /// </summary>
    /// <param name="priority">Between 100 (high), 200 (idle) and 300 (low)</param>
    /// <param name="action">Action which runs in main thread</param>
    public static void BeginInvoke(int priority, Action? action)
    {
        if (mainThreadId == Environment.CurrentManagedThreadId)
            action?.Invoke();
        else
        {
            var key = GtkDelegates.GetKey();
            OnePointerBoolRetDelegate? mainFunction = _ =>
            {
                action?.Invoke();
                mainFunction = null;    
                action = null;
                GtkDelegates.Remove(key);
                return false;
            };
            GtkDelegates.Add(key, mainFunction);
            var delegat = mainFunction as Delegate;
            var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
            IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
        }
    }

    public static void IdleAdd(int priority, Action action)
    {
        var key = GtkDelegates.GetKey();
        OnePointerBoolRetDelegate? mainFunction = _ =>
        {
            action.Invoke();
            // mainFunction = null;    
            // GtkDelegates.Remove(key);
            return true;
        };
        GtkDelegates.Add(key, mainFunction);
        var delegat = mainFunction as Delegate;
        var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
        IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
    }

    public static void SetTimer(int priority, TimeSpan timeout, Func<bool> action)
    {
        var key = GtkDelegates.GetKey();
        OnePointerBoolRetDelegate? mainFunction = _ =>
        {
            var ret = action.Invoke();
            if (!ret)
            {
                mainFunction = null;
                GtkDelegates.Remove(key);
            }
            return ret;
        };
        var delegat = mainFunction as Delegate;
        GtkDelegates.Add(key, delegat);
        var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
        SetTimer(priority, (int)timeout.TotalMilliseconds, funcPtr, IntPtr.Zero, IntPtr.Zero);
    }

    public static long SignalConnect<TDelegate>(this ObjectHandle obj, string name, TDelegate callback)
        where TDelegate : Delegate
    {
        // TODO Signal disconnect
        var key = GtkDelegates.GetKey();
        GtkDelegates.Add(key, callback);
        obj.AddWeakRefRaw(() => GtkDelegates.Remove(key));
        return SignalConnect(obj, name, Marshal.GetFunctionPointerForDelegate((Delegate)callback), IntPtr.Zero, 0);
    }

    public static void ShowDiagnostics()
    {
        GC.Collect();
        GC.Collect();
        Console.WriteLine($"Total memory: {System.Diagnostics.Process.GetCurrentProcess().WorkingSet64:N0}, managed: {GC.GetTotalMemory(true):N0}");

        var asyncReadies = GFile.GetAsyncReadyDelegates();
        var delegates = GtkDelegates.GetDelegatesCount();
        var actions = IActionMap.GetActionsCount();
        if (asyncReadies > 0)
            Console.WriteLine($"GFile AsyncReadies: {asyncReadies}");
        if (delegates > 0)
            Console.WriteLine($"Connected delegates: {delegates}");
        if (actions > 0)
            Console.WriteLine($"Connected actions: {actions}");
    }

    public static char KeyValToUnicode(int keyVal, int keyCode)
        => keyCode switch
        {
            22 => (char)ConsoleKey.Backspace,
            23 => (char)ConsoleKey.Tab,
            67 => (char)ConsoleKey.F1,
            68 => (char)ConsoleKey.F2,
            69 => (char)ConsoleKey.F3,
            70 => (char)ConsoleKey.F4,
            71 => (char)ConsoleKey.F5,
            72 => (char)ConsoleKey.F6,
            73 => (char)ConsoleKey.F7,
            74 => (char)ConsoleKey.F8,
            75 => (char)ConsoleKey.F9,
            76 => (char)ConsoleKey.F10,
            77 => (char)ConsoleKey.F11,
            78 => (char)ConsoleKey.F12,
            110 => (char)ConsoleKey.Home,
            112 => (char)ConsoleKey.PageUp,
            115 => (char)ConsoleKey.End,
            117 => (char)ConsoleKey.PageDown,
            118 => (char)ConsoleKey.Insert,
            _ => (char)_KeyValToUnicode(keyVal)
        };

    public static char RawKeyValToUnicode(int keyVal) => (char)_KeyValToUnicode(keyVal);

    internal static void Init() =>
        SynchronizationContext.SetSynchronizationContext(
            new GtkSynchronizationContext()
                .SideEffect(_ => mainThreadId = Environment.CurrentManagedThreadId));

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    extern static long SignalConnect(this ObjectHandle widget, string name, IntPtr callback, IntPtr obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnectAction(IntPtr action, string name, IntPtr callback, IntPtr obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint="g_signal_connect_object", CallingConvention = CallingConvention.Cdecl)]
    internal extern static long SignalConnectAction(ActionHandle action, string name, IntPtr callback, IntPtr obj, int n3);

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Main();

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_main_quit", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void MainQuit();

    // [DllImport(Libs.LibGtk, EntryPoint="gtk_init", CallingConvention = CallingConvention.Cdecl)]
    // public extern static void Init (ref int argc, ref IntPtr argv);

    [DllImport(Libs.LibGtk, EntryPoint = "g_signal_handler_disconnect", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SignalDisconnect(this ObjectHandle widget, long id);

    [DllImport(Libs.LibGtk, EntryPoint="g_idle_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void IdleAddFull(int priority, IntPtr func, IntPtr nil, IntPtr nil2);

    [DllImport(Libs.LibGtk, EntryPoint="g_timeout_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTimer(int priority, int intervalInMillis, nint func, nint nil, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_keyval_to_unicode", CallingConvention = CallingConvention.Cdecl)]
    extern static int _KeyValToUnicode(int keyVal);

    /// <summary>
    /// For usage in a non GTK app
    /// </summary> 
    static WindowHandle? nonGtkWindow;
    /// <summary>
    /// For usage in a non GTK app
    /// </summary> 
    static ApplicationHandle? nonGtkApp;

    static int mainThreadId;
}

