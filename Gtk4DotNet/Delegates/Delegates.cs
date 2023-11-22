//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
using System.Collections.Concurrent;
using LinqTools;

delegate void OnePointerDelegate(IntPtr p);
delegate void TwoPointerDelegate(IntPtr p, IntPtr pp);
delegate bool TwoPointerBoolRetDelegate(IntPtr p, IntPtr pp);
delegate void ThreePointerDelegate(IntPtr p, IntPtr pp, IntPtr ppp);
delegate void DrawFunctionDelegate(IntPtr drawingArea, IntPtr cairo, int width, int height, IntPtr data);
delegate void DrawingAreaResizeDelegate(IntPtr drawingArea, int width, int height, IntPtr data);
delegate void PressedGestureDelegate(IntPtr _, int pressCount, double x, double y, IntPtr __);
delegate void DragGestureDelegate(IntPtr _, double x, double y, IntPtr __);

static class Delegates
{
    public static long GetKey()
        => Interlocked.Increment(ref delegateKey);

    public static long Add(Delegate delegat)
        => Add(GetKey(), delegat);

    public static long Add(long key, Delegate delegat) 
    {
        delegates[key] = delegat;
        return key;
    } 

    public static void Remove(long key) 
        => delegates.TryRemove(key, out var _);

    public static long Remove(Delegate delegat)
    {
        var kvp = delegates.FirstOrDefault(n => n.Value == delegat);
        return kvp.Value != null
            ? kvp.Key.SideEffect(Remove)
            : -1;


    }

    static long delegateKey;
    static readonly ConcurrentDictionary<long, Delegate> delegates = new();
}

