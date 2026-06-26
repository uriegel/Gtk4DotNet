using System.Runtime.InteropServices;
using CsTools.Extensions;
using Gtk4DotNet.Internals;

namespace Gtk4DotNet;

// TODO ToDoApp with Adwaita
// TODO ColumnView: Sort columns
// TODO ColumnView: Filtering
// TODO DrawView

// TODO: Folder Widgets is release ready
// TODO: Folder Actions is release ready
// TODO: Folder Attributes is release ready
// TODO: Folder Enums is release ready
// TODO: Folder ErrorHandling is release ready
// TODO: Folder Extensions is release ready
// TODO: Folder Gestures is release ready
// TODO: Folder Gio is release ready

public static class Gtk
{
    public static Task InvokeAsync(Action action, bool highPriority = false)
        => InvokeAsync(action, highPriority ? 100 : 200);

    public static Task InvokeAsync(Action action, int priority)
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

    public static (int Major, int Minor, int Micro) GetVersion()
    {
        var ma = GetMajorVersion();
        var min = GetMinorVersion();
        var mi = GetMicroVersion();
        return (ma, min, mi);
    }

    public static Task<T> InvokeAsync<T>(Func<T> action, bool highPriority = false)
        => InvokeAsync(action, highPriority ? 100 : 200);

    public static Task<T> InvokeAsync<T>(Func<T> action, int priority)
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
            var key = GtkDelegates.Instance.GetKey("BeginInvoke");
            OnePointerBoolRetDelegate? mainFunction = _ =>
            {
                action?.Invoke();
                mainFunction = null;
                action = null;
                GtkDelegates.Instance.Remove(key.Key);
                return false;
            };
            GtkDelegates.Instance.Add(key, mainFunction);
            var delegat = mainFunction as Delegate;
            var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
            IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
        }
    }

    public static void IdleAdd(int priority, Action action)
    {
        // var key = GtkDelegates.Instance.GetKey();
        // OnePointerBoolRetDelegate? mainFunction = _ =>
        // {
        //     action.Invoke();
        //     // mainFunction = null;    
        //     // GtkDelegates.Instance.Remove(key);
        //     return true;
        // };
        // GtkDelegates.Instance.Add(key, mainFunction);
        // var delegat = mainFunction as Delegate;
        // var funcPtr = Marshal.GetFunctionPointerForDelegate(delegat);
        // IdleAddFull(priority, funcPtr, IntPtr.Zero, IntPtr.Zero);
    }

    public static bool GObjectTracing
    {
        get;
        set;
    }

    public static void ShowDiagnostics()
    {
        GC.Collect();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Console.WriteLine($"=========================================================================================");
        Console.WriteLine($"Gtk Version: {GetVersion()}");
        Console.WriteLine($"Total memory: {System.Diagnostics.Process.GetCurrentProcess().WorkingSet64:N0}, managed: {GC.GetTotalMemory(true):N0}");

        var registeredWidgets = Widget.GetRegisteredWidgetCount();
        var asyncReadies = AsyncReady.GetDelegateCount();
        var delegates = GtkDelegates.Instance.GetInfos();
        var gObjects = GObject.GObjectsDiagnostics.GetInfos();
        if (gObjects.Length > 0)
        {
            Console.WriteLine();
            foreach (var n in gObjects)
                Console.WriteLine($"Dangling GObjects: {n}");
        }
        if (delegates.Length > 0)
        {
            Console.WriteLine();
            foreach (var n in delegates)
                Console.WriteLine($"Connected delegates: {n}");
        }
        if (registeredWidgets > 0)
        {
            Console.WriteLine();
            Console.WriteLine($"Dangling widgets: {registeredWidgets}");
        }
        if (asyncReadies > 0)
        {
            Console.WriteLine();
            Console.WriteLine($"GFile AsyncReadies: {asyncReadies}");
        }
        Console.WriteLine($"=========================================================================================");
    }

    internal static bool Diagnostics
    {
        get;
        set;
    }

    internal static char KeyValToUnicode(int keyVal, int keyCode)
        => keyCode switch
        {
            22 => (char)ConsoleKey.Backspace,
            23 => (char)ConsoleKey.Tab,
            64 => (char)ConsoleKey.Enter,
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

    internal static char RawKeyValToUnicode(int keyVal) => (char)_KeyValToUnicode(keyVal);

    internal static void Init() =>
        SynchronizationContext.SetSynchronizationContext(
            new GtkSynchronizationContext()
                .SideEffect(_ => mainThreadId = Environment.CurrentManagedThreadId));

    [DllImport(Libs.LibGtk, EntryPoint = "g_idle_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void IdleAddFull(int priority, nint func, nint nil, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "g_timeout_add_full", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTimer(int priority, int intervalInMillis, nint func, nint nil, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_keyval_to_unicode", CallingConvention = CallingConvention.Cdecl)]
    extern static int _KeyValToUnicode(int keyVal);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_get_major_version", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetMajorVersion();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_get_minor_version", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetMinorVersion();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_get_micro_version", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetMicroVersion();

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