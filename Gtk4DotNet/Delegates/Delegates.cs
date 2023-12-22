//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
using System.Collections.Concurrent;
using CsTools.Extensions;
using GtkDotNet;

public delegate void ProgressCallback(long current, long total);

delegate void OnePointerDelegate(IntPtr p);
delegate void TwoPointerDelegate(IntPtr p, IntPtr pp);
delegate void ThreePointerDelegate(IntPtr p, IntPtr pp, IntPtr ppp);
delegate void DrawFunctionDelegate(IntPtr drawingArea, IntPtr cairo, int width, int height, IntPtr data);
delegate void DrawingAreaResizeDelegate(IntPtr drawingArea, int width, int height, IntPtr data);
delegate void PressedGestureDelegate(IntPtr _, int pressCount, double x, double y, IntPtr __);
delegate void DragGestureDelegate(IntPtr _, double x, double y, IntPtr __);
delegate bool KeyPressedDelegate(IntPtr _, uint key, uint keyCode, KeyModifiers keyModifiers, IntPtr __);
delegate void TwoLongAndPtrCallback(long current, long total, IntPtr zero);
delegate bool BoolRetDelegate();
delegate bool OnePointerBoolRetDelegate(IntPtr p);
delegate bool TwoPointerBoolRetDelegate(IntPtr p, IntPtr pp);

public static class GtkDelegates
{
    public static int Instances { get => delegates.Count; }
    internal static long GetKey()
        => Interlocked.Increment(ref delegateKey);

    internal static long Add(Delegate delegat)
        => Add(GetKey(), delegat);

    internal static long Add(long key, Delegate delegat) 
    {
        delegates[key] = delegat;
        return key;
    } 

    internal static void Remove(long key) 
        => delegates.TryRemove(key, out var _);

    internal static long Remove(Delegate delegat)
    {
        var kvp = delegates.FirstOrDefault(n => n.Value == delegat);
        return kvp.Value != null
            ? kvp.Key.SideEffect(Remove)
            : -1;
    }

    static long delegateKey;
    static readonly ConcurrentDictionary<long, Delegate> delegates = new();
}

