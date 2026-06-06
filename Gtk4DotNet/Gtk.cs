using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

public static class Gtk
{
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

    public static void IdleAdd(int priority, Action action)
    {
        // var key = GtkDelegates.GetKey();
        // OnePointerBoolRetDelegate? mainFunction = _ =>
        // {
        //     action.Invoke();
        //     // mainFunction = null;    
        //     // GtkDelegates.Remove(key);
        //     return true;
        // };
        // GtkDelegates.Add(key, mainFunction);
        // var delegat = mainFunction as Delegate;
        // var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
        // IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
    }

    public static void ShowDiagnostics()
    {
        GC.Collect();
        GC.Collect();
        Console.WriteLine($"Total memory: {System.Diagnostics.Process.GetCurrentProcess().WorkingSet64:N0}, managed: {GC.GetTotalMemory(true):N0}");

        // var asyncReadies = AsyncReady.GetDelegateCount();
        // var delegates = GtkDelegates.GetDelegatesCount();
        // var actions = IActionMap.GetActionsCount();
        // if (asyncReadies > 0)
        //     Console.WriteLine($"GFile AsyncReadies: {asyncReadies}");
        // if (delegates > 0)
        //     Console.WriteLine($"Connected delegates: {delegates}");
        // if (actions > 0)
        //     Console.WriteLine($"Connected actions: {actions}");
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
    internal extern static long SignalConnectAction(IntPtr action, string name, IntPtr callback, IntPtr obj, int n3);

    [DllImport(Libs.LibGtk, EntryPoint = "g_idle_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void IdleAddFull(int priority, IntPtr func, IntPtr nil, IntPtr nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "g_timeout_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTimer(int priority, int intervalInMillis, nint func, nint nil, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_keyval_to_unicode", CallingConvention = CallingConvention.Cdecl)]
    extern static int _KeyValToUnicode(int keyVal);

    static int mainThreadId;
}

public struct SignalData
{
    internal SignalData(long id, long key)
    {
        this.key = key;
        this.id = id;
    }
    internal long key;
    internal long id;
}