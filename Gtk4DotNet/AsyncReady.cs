using System.Collections.Concurrent;
using CsTools.Functional;
using GtkDotNet;

namespace GtkDotNet;

static class AsyncReady
{
    public static int GetDelegateCount() => Callbacks.Count;

    public readonly static Func<int> GetId = Incrementor.UseInt();

    public readonly static ConcurrentDictionary<int, ThreePointerDelegate> Callbacks = new();
    public readonly static ConcurrentDictionary<int, TwoLongAndPtrCallback> ProgressCallbacks = new();
}