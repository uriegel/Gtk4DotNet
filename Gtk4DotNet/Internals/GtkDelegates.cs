using System.Collections.Concurrent;
using CsTools.Extensions;

namespace Gtk4DotNet.Internals;

public delegate void ProgressCallback(long current, long total);
delegate void CustomSchemeRequestDelegate(nint request);
delegate void SoupMessageHeadersDelegate(string name, string value);

delegate void OnePointerDelegate(nint p);
delegate void TwoPointerDelegate(nint p, nint pp);
delegate void TwoPointerBoolDelegate(nint p, nint pp, bool b);
delegate void ThreePointerDelegate(nint p, nint pp, nint ppp);
delegate void DrawFunctionDelegate(nint drawingArea, nint cairo, int width, int height, nint data);
delegate void DrawingAreaResizeDelegate(nint drawingArea, int width, int height, nint data);
delegate void PressedGestureDelegate(nint _, int pressCount, double x, double y, nint __);
delegate void DragGestureDelegate(nint _, double x, double y, nint __);
//delegate void KeyReleasedDelegate(nint _, int key, int keyCode, KeyModifiers keyModifiers, nint __);
//delegate void OnModifiersDelegate(nint _, KeyModifiers keyModifiers, nint __);
delegate void TwoLongAndPtrCallback(long current, long total, nint zero);
delegate bool BoolRetDelegate();
delegate bool OnePointerBoolRetDelegate(nint p);
delegate bool TwoPointerBoolRetDelegate(nint p, nint pp);
delegate bool ThreePointerBoolRetDelegate(nint p, nint pp, nint ppp);
delegate void FourPointerDelegate(nint p1, nint p2, nint p3, nint p4);
delegate void PointerBoolDelegate(nint _, bool b);
delegate void PointerIntDelegate(nint _, int i);
delegate void AlertDialogResponseDelegate(nint p, string response, nint pp);
delegate bool KeyPressedDelegate(nint _, int key, int keyCode, KeyModifiers keyModifiers, nint __);

class GtkDelegates
{
    public static GtkDelegates Instance { get; } = new();
    public DelegateInfo[] GetInfos() => [.. delegates.Values]; 
    internal DelegateId GetKey(string name)
        => new(Interlocked.Increment(ref delegateKey), name);

    internal long Add(Delegate delegat, string name)
        => Add(GetKey(name).Key, delegat, name);

    internal long Add(DelegateId keyName, Delegate delegat, string? typeName = null)
        => Add(keyName.Key, delegat, keyName.Name, typeName);

    internal long Add(long key, Delegate delegat, string name, string? typeName = null)
    {
        delegates[key] = new DelegateInfo(delegat, name)
        {
            TypeName = typeName
        };
        return key;
    }

    internal void Remove(long key)
        => delegates.TryRemove(key, out var _);

    internal long Remove(Delegate delegat)
    {
        var kvp = delegates.FirstOrDefault(n => n.Value.Delegate == delegat);
        return kvp.Value.Delegate != null
            ? kvp.Key.SideEffect(Remove)
            : -1;
    }

    long delegateKey;

    readonly ConcurrentDictionary<long, DelegateInfo> delegates = [];
}

record struct DelegateInfo(Delegate Delegate, string Name)
{
    public string? TypeName { get; set; }
}

public struct DelegateId
{
    internal DelegateId(long key, string name, long signalId = 0)
    {
        Key = key;
        Name = name;
        SignalId = signalId;
    }
    
    internal long Key { get; }
    internal string Name { get; }
    internal long SignalId { get; set; }
}
