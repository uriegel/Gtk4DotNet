using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LinqTools;

namespace GtkDotNet;

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

