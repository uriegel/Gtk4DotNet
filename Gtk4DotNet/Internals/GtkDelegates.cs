using System.Collections.Concurrent;
using CsTools.Extensions;

namespace Gtk4DotNet.Internals;

public delegate void ProgressCallback(long current, long total);
delegate void CustomSchemeRequestDelegate(nint request);
delegate void SoupMessageHeadersDelegate(string name, string value);

delegate void OnePointerDelegate(nint p);
delegate void TwoPointerDelegate(nint p, nint pp);
delegate void ThreePointerDelegate(nint p, nint pp, nint ppp);
delegate void DrawFunctionDelegate(nint drawingArea, nint cairo, int width, int height, nint data);
delegate void DrawingAreaResizeDelegate(nint drawingArea, int width, int height, nint data);
delegate void PressedGestureDelegate(nint _, int pressCount, double x, double y, nint __);
delegate void DragGestureDelegate(nint _, double x, double y, nint __);
//delegate bool KeyPressedDelegate(nint _, int key, int keyCode, KeyModifiers keyModifiers, nint __);
//delegate void KeyReleasedDelegate(nint _, int key, int keyCode, KeyModifiers keyModifiers, nint __);
//delegate void OnModifiersDelegate(nint _, KeyModifiers keyModifiers, nint __);
delegate void TwoLongAndPtrCallback(long current, long total, nint zero);
delegate bool BoolRetDelegate();
delegate bool OnePointerBoolRetDelegate(nint p);
delegate bool TwoPointerBoolRetDelegate(nint p, nint pp);
delegate bool ThreePointerBoolRetDelegate(nint p, nint pp, nint ppp);
delegate void PointerBoolDelegate(nint _, bool b);
delegate void PointerIntDelegate(nint _, int i);
delegate void AlertDialogResponseDelegate(nint p, string response, nint pp);

static class GtkDelegates
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

    internal static int GetDelegatesCount() => delegates.Count;
    static readonly ConcurrentDictionary<long, Delegate> delegates = [];
}

